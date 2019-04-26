using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OSCiR.Areas.Admin.Class.Model;
using OSCiR.Areas.Shared;
using OSCiR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OSCiR.Areas.Shared.Auth;
using OSCiR.Shared;
using OSCiR.Datastore;
using App;

namespace OSCiR.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/class")]
    public class ClassController : ControllerBase
    {
        private BlueprintManager _blueprintManager;
        private IAuthorizationService _authorizationService;

        public ClassController(CMDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _blueprintManager = new BlueprintManager(new BlueprintRepository(dbContext));
            _authorizationService = authorizationService;
        }



        [HttpGet]
        public async Task<ActionResult<ClassEntity[]>> GetList(string classNameContains, string classCategoryEquals)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var classReply = _blueprintManager.ReadClasses(classNameContains, classCategoryEquals);
                return Ok(classReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpGet("{classGuid}")]
        public async Task<ActionResult<ClassEntity>> Get(Guid classGuid)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var classReply = _blueprintManager.ReadClasses(new Guid[] { classGuid }).FirstOrDefault();

                return Ok(classReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// Returns a list of specific Classes
        /// 
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="classEntityIds">Class entity identifiers.</param>
        [HttpPost("list")]
        public async Task<ActionResult<IEnumerable<ClassEntity>>> Get([FromBody] Guid[] classEntityIds)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var classReply = _blueprintManager.ReadClasses(classEntityIds);
                return Ok(classReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpPut("{classEntityId}")]
        public async Task<ActionResult<ClassEntity>> Put(Guid classEntityId, [FromBody] ClassEntity classEntity)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, classEntity, Operations.Update);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {

                classEntity.Id = classEntityId;
                var userId = Utils.getCurrentUserName(User);
                if (classEntity.Category == null) classEntity.Category = "";
                classEntity.ModifiedBy = userId;


                var classReply = _blueprintManager.UpdateClass(classEntity);

                return Ok(classReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpPost()]
        public async Task<ActionResult<ClassEntity>> Post([FromBody] ClassEntity classEntity)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, classEntity, Operations.Create);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var userName = Utils.getCurrentUserName(User);
                if (classEntity.Category == null) classEntity.Category = "";
                if (classEntity.CreatedBy == null) classEntity.CreatedBy = userName;
                if (classEntity.ModifiedBy == null) classEntity.ModifiedBy = userName;

                var classReply = _blueprintManager.CreateClass(classEntity);
                return Created("/" + classReply.Id, classReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpDelete("{classGuid}")]
        public async Task<ActionResult<object>> Delete(Guid classGuid)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassEntity(), Operations.Delete);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                if (!_blueprintManager.DeleteClass(classGuid))
                {
                    return BadRequest("DeleteClass() failed");
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
