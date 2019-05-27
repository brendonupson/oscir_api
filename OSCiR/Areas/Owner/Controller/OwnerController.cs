using System;
using System.Threading.Tasks;
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
    [Route("api/owner")]
    public class OwnerController : ControllerBase
    {
        private OwnerManager _ownerManager;
        private IAuthorizationService _authorizationService;

        public OwnerController(CMDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _ownerManager = new OwnerManager(new OwnerRepository(dbContext));
            _authorizationService = authorizationService;
        }



        [HttpGet]
        public async Task<ActionResult<OwnerEntity[]>> Get(string ownerCodeEquals, string ownerNameContains)
        {
            //TODO paging result
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new OwnerEntity(), Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var ownerReply = _ownerManager.GetOwners(ownerCodeEquals, ownerNameContains);

                return Ok(ownerReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpGet("{ownerGuid}")]
        public ActionResult<OwnerEntity> Get(Guid ownerGuid)
        {
            try
            {
                var ownerReply = _ownerManager.Read(ownerGuid);
                return Ok(ownerReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpPut("{ownerGuid}")]
        public async Task<ActionResult<OwnerEntity>> Put([FromRoute] Guid ownerGuid, [FromBody] OwnerEntity ownerEntity)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, ownerEntity, Operations.Create);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                ownerEntity.Id = ownerGuid;
                var userName = Utils.getCurrentUserName(User);
                ownerEntity.ModifiedBy = userName;

                var ownerReply = _ownerManager.Update(ownerEntity);
                return Ok(ownerReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpPost()]
        public async Task<ActionResult<OwnerEntity>> Post([FromBody] OwnerEntity ownerEntity)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, ownerEntity, Operations.Update);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                var userName = Utils.getCurrentUserName(User);

                if (ownerEntity.CreatedBy == null) ownerEntity.CreatedBy = userName;
                if (ownerEntity.ModifiedBy == null) ownerEntity.ModifiedBy = userName;

                var ownerReply = _ownerManager.Create(ownerEntity);
                return Created("/" + ownerReply.Id, ownerReply);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpDelete("{ownerGuid}")]
        public async Task<ActionResult<object>> Delete(Guid ownerGuid)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new OwnerEntity(), Operations.Delete);
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                if (!_ownerManager.Delete(ownerGuid))
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
