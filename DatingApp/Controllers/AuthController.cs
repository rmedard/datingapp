using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto user)
        {
            if (!string.IsNullOrEmpty(user.Username))
            {
                user.Username = user.Username.ToLower();
            }

            if (await _repository.UserExists(user.Username))
            {
                ModelState.AddModelError("Username", "Username is already taken!!");
            }

            //Validate model: Annotations in the entity
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = new User
            {
                Username = user.Username
            };

            var createdUser = await _repository.Register(userToCreate, user.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLogin)
        {
            var user = await _repository.Login(userForLogin.Username.ToLower(), userForLogin.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            //Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new {tokenString});
        }
    }
}
