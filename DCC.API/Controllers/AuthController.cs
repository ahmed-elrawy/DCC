using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DCC.API.Model;
using DCC.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using DatingApp.API.Dtos;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
 using System.Web;
using Newtonsoft.Json;

namespace DCC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        #region privateMembers
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        #endregion
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        #region ctor
        public AuthController(IConfiguration config,
         IMapper mapper, UserManager<User> userManager,
         SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
        }
        #endregion

        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userRegisterDto)
        {
            var userToCreate = _mapper.Map<User>(userRegisterDto);
            var result = await _userManager.CreateAsync(userToCreate, userRegisterDto.Password);
            var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);
            if (result.Succeeded)
            {
                
                return CreatedAtRoute("GetUser", new { Controller = "Users", id = userToCreate.Id }, userToReturn);

            }
            return BadRequest(result.Errors);
        }

        #endregion

        #region Loing
        [HttpPost("login")]
        public async Task<IActionResult> Loing(UserForLoginDto userLoginDTO)
        {
            var user = await _userManager.FindByNameAsync(userLoginDTO.Username);
            var result = await _signInManager
            .CheckPasswordSignInAsync(user, userLoginDTO.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == userLoginDTO.Username.ToUpper());
                var userReturn = _mapper.Map<UserForListDto>(appUser);
                return Ok(new
                {
                    token = GenerateToken(appUser),
                    user = userReturn
                });

            }

            return Unauthorized();


        }
        #endregion
        private async Task<string> GenerateToken(User user)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSetting:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandelar = new JwtSecurityTokenHandler();


            var token = tokenHandelar.CreateToken(tokenDescriptor);
            return tokenHandelar.WriteToken(token);
        }
        #region EmailExists
        [HttpGet("checkUserName")]
        public async Task<bool> EmailExists(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);
            if (result != null)
                return true;

            return false;
        }
        #endregion

    }
}
