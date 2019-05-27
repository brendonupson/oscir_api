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

namespace OSCiR.Areas.Admin.Class.Controller
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/classdef")]
    public class ClassDefController : ControllerBase
    {
        private BlueprintManager _blueprintManager;
        private IAuthorizationService _authorizationService;

        public ClassDefController(CMDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _blueprintManager = new BlueprintManager(new BlueprintRepository(dbContext), new ConfigItemRepository(dbContext));
            _authorizationService = authorizationService;
        }

        [HttpGet("{classEntityId}/apiexample")]
        public async Task<ActionResult<ConfigItemEntity>> Get([FromRoute] Guid classEntityId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var exampleReply = _blueprintManager.GetConfigItemExample(classEntityId);
                return Ok(exampleReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("{classEntityId}")]
        public async Task<ActionResult<ClassEntity>> GetRaw([FromRoute] Guid classEntityId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClassEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var exampleReply = _blueprintManager.GetClassFullPropertyDefinition(classEntityId);
                return Ok(exampleReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


    }
}
