using System;
using System.Security.Claims;
using App;
using OSCiR.Areas.Shared;
using OSCiR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OSCiR.Datastore;
using OSCiR.Shared;

namespace OSCiR.Areas.Auth.Controller
{
    [Authorize]
    [ApiController]
    [Route("Auth")]
    public class AuthenticationController : ControllerBase
    {
        private UserManager _userManager;

        public AuthenticationController(CMDbContext dbContext, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _userManager = new UserManager(new UserRepository(dbContext), appSettings.Value.Secret);

        }

        [HttpGet("User")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = _userManager.GetUser(new Guid(userId));
                if (user == null)
                {
                    return BadRequest("User " + userId + " not found");
                }

                return Ok(new
                {
                    token = _userManager.GenerateToken(user),
                    username = user.Username,
                    firstname = user.FirstName,
                    lastname = user.LastName,
                    lastLogin = user.LastLogin,
                    id = user.Id,
                    isAdmin = user.IsAdmin
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Authenticate([FromBody]UserLogin userParam)
        {
            if (userParam.Username == null || userParam.Username.Length == 0) return BadRequest("Invalid username");
            if (userParam.Password == null || userParam.Password.Length == 0) return BadRequest("Invalid password");

            try
            {
                var user = _userManager.Authenticate(userParam.Username, userParam.Password);

                if (user == null)
                    return Unauthorized(new { message = "Username or password is incorrect" });

                return Ok(new
                {
                    token = user.Token,
                    username = user.Username,
                    firstname = user.FirstName,
                    lastname = user.LastName,
                    lastLogin = user.LastLogin,
                    id = user.Id,
                    isAdmin = user.IsAdmin
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }



    }
}
