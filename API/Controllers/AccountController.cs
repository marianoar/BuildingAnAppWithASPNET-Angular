using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _dataContext;
        public AccountController(DataContext context) {

        _dataContext = context;

        }

        [HttpPost("register")] //api/account/register
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {

            if(await UserExists(registerDto.Username)) {
                return BadRequest("Username is taken");
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return Ok(user);
        }

        private async Task<bool> UserExists(string username)
        {
            return await _dataContext.Users.AnyAsync(u => u.UserName == username.ToLower());
        }

    }
}
