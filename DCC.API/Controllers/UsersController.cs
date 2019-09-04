using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using DCC.API.Dtos;
using DCC.API.Helper;
using DCC.API.Model;
using DCC.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        #region privateMembers
        private readonly IDccRepository _repo;
        private readonly IMapper _mapper;

        #endregion

        #region ctor
        public UsersController(IDccRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        #endregion

        #region GetUsers
 
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var userToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(userToReturn);
        }
        #endregion


        #region GetUsersPaging
 
        [HttpGet("getPaging", Name = "GetUsersPaging")]
        public async Task<IActionResult> GetUsersPaging([FromQuery]UserParams userParams)
        {
            var currentUserId =  User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;
            if (string.IsNullOrEmpty(userParams.typeOfUser))
            {
                userParams.typeOfUser = userFromRepo.TypeOfUser == "Doctors" ? "Patient" : "Doctors";
            }
            var users = await _repo.GetUsersPaging(userParams);
            var userToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize,
            users.TotalCount, users.TotalPages);
            return Ok(userToReturn);
        }
        #endregion

        #region GetUser
 
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string id)
        {

            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        #endregion

        #region UpdateUser
 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserForUpdateDto userUpdateDto)
        {
            if (id !=  User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            var userFromRepo = await _repo.GetUser(id);
            _mapper.Map(userUpdateDto, userFromRepo);
            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updation User With {id} faild on save");
        }
        #endregion


        #region LikeUser
 
        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(string id, string recipientId)
        {
            if (id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            var like = await _repo.GetLike(id, recipientId);
            if (like != null)
                return BadRequest("You Already Like This User");
            if (await _repo.GetUser(recipientId) == null)
                return NotFound();

            like = new DCC.API.Model.Like
            {
                LikerId = id,
                LikeeId = recipientId

            };
            _repo.Add<Like>(like);
            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Faild To Like User !");
        }

        #endregion

  
    }

}