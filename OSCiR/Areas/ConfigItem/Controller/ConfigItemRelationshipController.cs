using System;
using System.Threading.Tasks;
using OSCiR.Areas.Admin.Class.Model;
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
    [Route("api/cirelationship")]
    public class ConfigItemRelationshipController : ControllerBase
    {

        private ConfigItemManager _configItemManager;
        private IAuthorizationService _authorizationService;

        public ConfigItemRelationshipController(CMDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _configItemManager = new ConfigItemManager(new ConfigItemRepository(dbContext), new BlueprintRepository(dbContext));
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get all relationships for the given CI parameters. 
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="sourceConfigItemId">Source config item identifier.</param>
        /// <param name="targetConfigItemId">Target config item identifier. This may be Empty</param>
        [HttpGet()]
        public async Task<ActionResult<ConfigItemRelationshipEntity>> Get(Guid sourceConfigItemId, Guid targetConfigItemId)
        {
            //FIXME is this used?
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassRelationshipEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var relationshipReply = _configItemManager.GetConfigItemRelationships(sourceConfigItemId, targetConfigItemId);
                return Ok(relationshipReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpGet("{configItemRelationshipId}")]
        public async Task<ActionResult<ConfigItemRelationshipEntity>> Get(Guid configItemRelationshipId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassRelationshipEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var relationshipReply = _configItemManager.ReadConfigItemRelationship(configItemRelationshipId);
                return Ok(relationshipReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        //No update (POST), just delete then re-add the relationship


        [HttpPost()]
        public async Task<ActionResult<ConfigItemRelationshipEntity>> Post([FromBody] ConfigItemRelationshipEntity configItemRelationship)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, configItemRelationship, Operations.Update);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                if (configItemRelationship.SourceConfigItemEntityId == Guid.Empty)
                {
                    return BadRequest("Source ConfigItem not specified");
                }

                if (configItemRelationship.TargetConfigItemEntityId == Guid.Empty)
                {
                    return BadRequest("Target ConfigItem not specified");

                }

                var userName = Utils.getCurrentUserName(User);
                var relationshipReply = _configItemManager.CreateConfigItemRelationship(configItemRelationship.SourceConfigItemEntityId, configItemRelationship.TargetConfigItemEntityId, configItemRelationship.RelationshipDescription, userName);

                return Ok(relationshipReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpDelete("{configItemRelationshipId}")]
        public async Task<ActionResult<object>> Delete(Guid configItemRelationshipId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ConfigItemRelationshipEntity(), Operations.Delete);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                if (!_configItemManager.DeleteConfigItemRelationship(configItemRelationshipId))
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
