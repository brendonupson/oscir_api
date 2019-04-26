using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSCiR.Areas.Admin.Class.Model;
using OSCiR.Areas.Shared;
using OSCiR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OSCiR.Shared;
using App;
using OSCiR.Datastore;

namespace OSCiR.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/configitem")]
    public class ConfigItemController : ControllerBase
    {
        private ConfigItemManager _configItemManager;
        private IAuthorizationService _authorizationService;

        public ConfigItemController(CMDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _configItemManager = new ConfigItemManager(new ConfigItemRepository(dbContext), new BlueprintRepository(dbContext));
            _authorizationService = authorizationService;
        }



        [HttpGet]
        public async Task<ActionResult<ConfigItemEntity[]>> GetCIsForClassOrOwner(Guid? classEntityId, Guid? ownerId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ConfigItemEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var ciReply = _configItemManager.GetConfigItemsForClassOrOwner(classEntityId, ownerId);
                return Ok(ciReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpGet("{configItemId}")]
        public async Task<ActionResult<ConfigItemEntity>> Get(Guid configItemId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ConfigItemEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var ciReply = _configItemManager.ReadConfigItems(new Guid[] { configItemId }).FirstOrDefault();
                return Ok(ciReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpPost("list")]
        public async Task<ActionResult<IEnumerable<ConfigItemEntity>>> Get([FromBody] Guid[] configItemEntityIds)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ConfigItemEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var ciReply = _configItemManager.ReadConfigItems(configItemEntityIds);
                return Ok(ciReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpPut("{configItemId}")]
        public async Task<ActionResult<ConfigItemEntity>> Put([FromRoute] Guid configItemId, [FromBody] ConfigItemEntity configItem)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, configItem, Operations.Update);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                configItem.Id = configItemId;
                var userName = Utils.getCurrentUserName(User);
                configItem.ModifiedBy = userName;

                var ciReply = _configItemManager.UpdateConfigItem(configItem);

                return Ok(ciReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

        }


        [HttpPost()]
        public async Task<ActionResult<ConfigItemEntity>> Post(ConfigItemEntity configItem)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, configItem, Operations.Create);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {

                var userName = Utils.getCurrentUserName(User);

                if (configItem.CreatedBy == null) configItem.CreatedBy = userName;
                if (configItem.ModifiedBy == null) configItem.ModifiedBy = userName;

                var ciReply = _configItemManager.CreateConfigItem(configItem);
                return Created("/" + ciReply.Id, ciReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpDelete("{configItemId}")]
        public async Task<object> Delete(Guid configItemId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ConfigItemEntity(), Operations.Delete);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                if (!_configItemManager.DeleteConfigItem(configItemId))
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
