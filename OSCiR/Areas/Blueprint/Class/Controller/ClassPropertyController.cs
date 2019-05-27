using System;
using System.Threading.Tasks;
using OSCiR.Areas.Shared;
using OSCiR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OSCiR.Shared;
using OSCiR.Datastore;
using App;
using OSCiR.Areas.Admin.Class.Model;

namespace OSCiR.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/classproperty")]
    public class ClassPropertyController : ControllerBase
    {
        private BlueprintManager _blueprintManager;
        private IAuthorizationService _authorizationService;

        public ClassPropertyController(CMDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _blueprintManager = new BlueprintManager(new BlueprintRepository(dbContext), new ConfigItemRepository(dbContext));
            _authorizationService = authorizationService;
        }



        // POST api/values
        [HttpPut]
        public async Task<ActionResult<ClassPropertyEntity>> Put([FromBody] ClassPropertyEntity classPropertyEntity)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassPropertyEntity(), Operations.Update);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var userName = Utils.getCurrentUserName(User);

                classPropertyEntity.ModifiedBy = userName;
                var propertyReply = _blueprintManager.UpdateClassProperty(classPropertyEntity);
                return Ok(propertyReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        // PUT api/values/5
        //[HttpPut("{id}")]
        [HttpPost()]
        public async Task<ActionResult<ClassPropertyEntity>> Post([FromBody] ClassPropertyEntity classPropertyEntity)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassPropertyEntity(), Operations.Create);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var userName = Utils.getCurrentUserName(User);
                if (classPropertyEntity.CreatedBy == null) classPropertyEntity.CreatedBy = userName;
                if (classPropertyEntity.ModifiedBy == null) classPropertyEntity.ModifiedBy = userName;

                var propertyReply = _blueprintManager.CreateClassProperty(classPropertyEntity);

                return Created("/" + propertyReply.Id, propertyReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpDelete("{classPropertyGuid}")]
        public async Task<ActionResult<object>> Delete(Guid classPropertyGuid)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassPropertyEntity(), Operations.Delete);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                if (!_blueprintManager.DeleteClassProperty(classPropertyGuid))
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
