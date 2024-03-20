using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Dto;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UsersController(IUserRepository _userRepository, IMapper _mapper, IConfiguration _configuration)
        {
            this._mapper = _mapper;
            this._configuration = _configuration;
            this._userRepository = _userRepository;
        }

        [HttpPost("Register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(ModelState);

            var previousUser = _userRepository.GetUser(_mapper.Map<UserDto>(user)).
                Where(u => u.Email == user.Email);

            if (previousUser.Count() > 0)
            {
                ModelState.AddModelError("", "Email already in use.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            if (!_userRepository.CreateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong when creating a user");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }

        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult LogInUser([FromBody] UserDto user)
        {
            if (user == null)
                return BadRequest(ModelState);

            var previousUser = _userRepository.GetUser(user).Where(u => u.Email == user.Email);

            if (previousUser.Count() == 0)
            {
                ModelState.AddModelError("", "Bad credentials.");
                return StatusCode(404, ModelState);
            }

            var foundUser = previousUser.First();

            if (!BCrypt.Net.BCrypt.Verify(user.Password, foundUser.Password))
            {
                ModelState.AddModelError("", "Bad credentials");
                return StatusCode(400, ModelState);
            }

            string token = CreateToken(foundUser);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            //Fields that are stored in JWT
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Role, user.Id.ToString())
            };
            
            //Getting the secret key from appsettings.json
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!)
            );

            //Algo to use in JWT signa.
            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            //Build the token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: cred   
            );

            //Write the token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }
    }
}