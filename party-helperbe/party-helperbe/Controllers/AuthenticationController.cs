using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using party_helperbe.Common.RequestModels;
using party_helperbe.Common.Utils;
using party_helperbe.DataAccess.Models;
using System.Security.Claims;

namespace party_helperbe.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly PgSQLDbContext _appData;

        public AuthenticationController(PgSQLDbContext appData)
        {
            _appData = appData;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterModel request)
        {
            if(string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.userName)||string.IsNullOrEmpty(request.emailAddress)) 
            {
                return BadRequest(new { error = "Empty request" });
            }

            bool found=_appData.Members.Any(m=>m.emailAddress == request.emailAddress);

            if(found)
            {
                return BadRequest(new { error = "Email already exist" });
            }

            request.Password=PasswordHelper.EncryptPassword(request.Password);

            var user = new Member
            {
                emailAddress = request.emailAddress,
                userName = request.userName,
                Password = request.Password
            };
            await _appData.Members.AddAsync(user);
            await _appData.SaveChangesAsync();

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim("memberId",user.memberId.ToString())
            };

            var token=TokenHelper.GenerateJwtToken(claims);

            return Ok(new {Token=token});
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {

            if (string.IsNullOrEmpty(request.emailAdress)||string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { error = "Empty request" });
            }

            if(request.emailAdress == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
            {
                if(request.Password != Environment.GetEnvironmentVariable("ADMIN_PASSWORD"))
                {
                    return Unauthorized(new { error = "Password for admin is not correct" });
                }

                var adminClaims = new[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("memberId", "0")
                };

                var adminToken=TokenHelper.GenerateJwtToken(adminClaims);
                return Ok(new {Token=adminToken});
            }


            var user = await _appData.Members.FirstOrDefaultAsync(m => m.emailAddress == request.emailAdress);

            if(user == null) 
            { 
                return BadRequest(new { error = "The user was not found" }); 
            }

            
        
            if (PasswordHelper.VerifyPassword(request.Password,user.Password)==false) 
            {
                return BadRequest(new { error = "Password is wrong" });
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim("memberId",user.memberId.ToString())
            };

            var token=TokenHelper.GenerateJwtToken(claims);

            return Ok(new { Token = token });

        }

        [HttpGet("role")]
        [Authorize(Roles ="Admin,User")]
        public IActionResult GetRole()
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;

            return Ok( new { Role = role });
        }



    }
}
