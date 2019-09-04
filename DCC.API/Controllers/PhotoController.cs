using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Dtos;
using DCC.API.Dtos;
using DCC.API.Helper;
using DCC.API.Model;
using DCC.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DCC.API.Controllers
{
     
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        #region privateMembers
        private readonly IDccRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        #endregion

        #region ctor
        public PhotoController(IDccRepository repo,
    IMapper mapper,
    IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }
        #endregion

        #region GetPhoto

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }
        #endregion


        #region AddPhotoForUser

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(string userId,
        [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            // cheack user id in rout same user id in token #endregion
            if (userId !=  User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            //get user data
            var userFromRepo = await _repo.GetUser(userId);
            // instans file
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            // cheack any photo is comming 
            if (file.Length > 0)
            {
                // create object to upload if have atlest one photo 
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                        .Crop("fill")
                        .Gravity("face")
                    };
                    // upload photo 
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            if (!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;
            userFromRepo.Photos.Add(photo);

            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }
            return BadRequest("Could Not  add the photo");
        }
        #endregion

        #region SetMainPhoto
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(string userId, int id)
        {
            if (userId !=  User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            //get user data
            var userFromRepo = await _repo.GetUser(userId);

            if (!userFromRepo.Photos.Any(u => u.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This Photo Is Already Main To Your Profile ");

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Colud Not Set The Photo Main");



          }  
          #endregion

            #region DeletePhoto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(string userId, int id)
        {

            if (userId !=  User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            //get user data
            var userFromRepo = await _repo.GetUser(userId);

            if (!userFromRepo.Photos.Any(u => u.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest(" Cannot Delete The Main Photo ! ");

            if (photoFromRepo.PublicId != null)
            {
                var deletParam = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deletParam);
                if (result.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }
            }
            if (photoFromRepo.PublicId == null)
            {
                _repo.Delete(photoFromRepo);

            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Fail To Delete a Photo Please Try Agin ! ");
        }
        #endregion

    }
}