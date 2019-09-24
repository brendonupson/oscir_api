using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using ApplicationLayer.App;
using DomainLayer.Model.AdHoc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OSCiR.Areas.Admin.Class.Model;
using OSCiR.Areas.Shared;
using OSCiR.Datastore;
using OSCiR.Model;
using OSCiR.Shared;

namespace OSCiR.Areas.Report.Controller
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private ReportManager _reportManager;
        private IAuthorizationService _authorizationService;

        public ReportController(CMDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _reportManager = new ReportManager(new ConfigItemRepository(dbContext), new OwnerRepository(dbContext));
            _authorizationService = authorizationService;
        }


        [HttpGet("ConfigItemsByOwner")]
        public async Task<ActionResult<IEnumerable<OwnerStatistic>>> GetConfigItemsByOwner(Guid? ownerEntityId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ConfigItemEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var ciReply = _reportManager.GetOwnerStatistics(ownerEntityId);
                return Ok(ciReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("ConfigItemsByClass")]
        public async Task<ActionResult<IEnumerable<OwnerStatistic>>> GetConfigItemsByClass(Guid? classEntityId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ConfigItemEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var ciReply = _reportManager.GetClassStatistics(classEntityId);
                return Ok(ciReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /*
        [HttpGet("ConfigItemsModifiedByDay")]
        public async Task<ActionResult<IEnumerable<OwnerStatistic>>> GetConfigItemsByClass(DateTime since)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ConfigItemEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var ciReply = _reportManager.GetClassStatistics(classEntityId);
                return Ok(ciReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }*/

    }//class
}
