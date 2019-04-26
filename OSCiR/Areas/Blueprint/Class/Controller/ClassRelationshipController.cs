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
    [Route("api/classrelationship")]
    public class ClassRelationshipController : ControllerBase
    {

        private BlueprintManager _blueprintManager;
        private IAuthorizationService _authorizationService;

        public ClassRelationshipController(CMDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _blueprintManager = new BlueprintManager(new BlueprintRepository(dbContext));
            _authorizationService = authorizationService;
        }


        [HttpGet("{classRelationshipGuid}")]
        public async Task<ActionResult<ClassRelationshipEntity>> Get(Guid classRelationshipGuid)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassRelationshipEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var relationshipReply = _blueprintManager.ReadClassRelationship(classRelationshipGuid);
                return Ok(relationshipReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        //No update (POST), just delete then re-add the relationship

        [HttpPost()]
        public async Task<ActionResult<ClassRelationshipEntity>> Post([FromBody] ClassRelationshipEntity classRelationshipEntity)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, classRelationshipEntity, Operations.Create);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {

                var userName = Utils.getCurrentUserName(User);

                if (classRelationshipEntity.CreatedBy == null) classRelationshipEntity.CreatedBy = userName;
                if (classRelationshipEntity.ModifiedBy == null) classRelationshipEntity.ModifiedBy = userName;

                var relationshipReply = _blueprintManager.CreateClassRelationship(classRelationshipEntity);
                return Ok(relationshipReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpDelete("{classRelationshipGuid}")]
        public async Task<ActionResult<object>> Delete(Guid classRelationshipGuid)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassRelationshipEntity(), Operations.Delete);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                if (!_blueprintManager.DeleteClassRelationship(classRelationshipGuid))
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
