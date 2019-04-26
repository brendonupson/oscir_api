using System;
using System.Security.Claims;
using System.Threading.Tasks;
using App;
using OSCiR.Areas.Shared;
using OSCiR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OSCiR.Datastore;
using OSCiR.Shared;

namespace OSCiR.Areas.Auth.Controller
{

    [Authorize]
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private UserManager _userManager;
        private IAuthorizationService _authorizationService;

        public UserController(CMDbContext dbContext, IOptions<AppSettings> appSettings, IAuthorizationService authorizationService)
        {
            _userManager = new UserManager(new UserRepository(dbContext), appSettings.Value.Secret);
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<UserEntity>> GetAll()
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new UserEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var userReply = _userManager.GetAllUsers();
                return Ok(userReply);
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserEntity>> Get(Guid id)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new UserEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var userReply = _userManager.GetUser(id);
                return Ok(userReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<UserEntity>> Put([FromRoute] Guid id, [FromBody] UserEntity userEntity, string newPassword)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, userEntity, Operations.Update);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                userEntity.Id = id;
                var userName = Utils.getCurrentUserName(User);
                userEntity.ModifiedBy = userName;

                bool updatePassword = newPassword != null && newPassword.Length > 0;
                if (updatePassword) userEntity.SetPassword(newPassword);


                var userReply = _userManager.UpdateUser(userEntity, updatePassword); 
                return Ok(userReply);
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpPost()]
        public async Task<ActionResult<UserEntity>> Post([FromBody] UserEntity userEntity, string newPassword)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, userEntity, Operations.Create);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var userName = Utils.getCurrentUserName(User);

                if (userEntity.CreatedBy == null) userEntity.CreatedBy = userName;
                if (userEntity.ModifiedBy == null) userEntity.ModifiedBy = userName;

                if (newPassword != null && newPassword.Length > 0) userEntity.SetPassword(newPassword);

                var userReply = _userManager.Create(userEntity);
                return Created("/" + userReply.Id, userReply);
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> Delete(Guid id)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new UserEntity(), Operations.Delete);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                if (!_userManager.Delete(id))
                {
                    return BadRequest("Delete failed");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

    }
}
