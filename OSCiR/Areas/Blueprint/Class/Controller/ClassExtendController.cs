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

namespace OSCiR.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/classextend")]
    public class ClassExtendController : ControllerBase
    {
        private BlueprintManager _blueprintManager;
        private IAuthorizationService _authorizationService;

        public ClassExtendController(CMDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _blueprintManager = new BlueprintManager(new BlueprintRepository(dbContext));
            _authorizationService = authorizationService;
        }



        [HttpPost()]
        public async Task<ActionResult<ClassExtendEntity>> Post([FromBody] ClassExtendEntity classExtendEntity)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, classExtendEntity, Operations.Create);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {

                var userName = Utils.getCurrentUserName(User);
                if (classExtendEntity.CreatedBy == null) classExtendEntity.CreatedBy = userName;
                if (classExtendEntity.ModifiedBy == null) classExtendEntity.ModifiedBy = userName;

                var extendReply = _blueprintManager.CreateClassExtend(classExtendEntity);
                return Created("/" + extendReply.Id, extendReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpDelete("{classExtendGuid}")]
        public async Task<ActionResult<object>> Delete([FromRoute] Guid classEntityId, Guid classExtendGuid)
        {

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassExtendEntity(), Operations.Delete);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                if (!_blueprintManager.DeleteClassExtend(classExtendGuid))
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
