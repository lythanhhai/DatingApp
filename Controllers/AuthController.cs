using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data.Entities;
using DatingApp.API.DTOs;
using DatingApp.API.Data;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly DataContext _context;
        public AuthController(DataContext dbContext)
        {
            this._context = dbContext;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthUserDto authUserDto)
        {
            authUserDto.Username = authUserDto.Username.ToLower();
            if(_context.User.Any(u => u.Username == authUserDto.Username))
            {
                return BadRequest("Username is already existed!");
            }
            using var hmac = new HMACSHA512();
            var passwordByes = Encoding.UTF8.GetBytes(authUserDto.Password);
            var newUser = new Users {
                Username = authUserDto.Username,
                PasswordHash = hmac.ComputeHash(passwordByes),
                PasswordSalt = hmac.Key,
            };
            _context.User.Add(newUser);
            _context.SaveChanges();
            return Ok(newUser.Username);
        }

        [HttpPost("login")]
        public void Login([FromBody] string value)
        {
        }


    }
}