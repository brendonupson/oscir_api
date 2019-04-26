using System;
using System.Linq;
using App;
using OSCiR.Areas.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OSCiR.Datastore;
using OSCiR.Shared;

namespace OSCiR.Areas.Setup.Controller
{
    [Authorize]
    [ApiController]
    [Route("Setup")]
    public class Setup : ControllerBase
    {
        //private UserRepository _userRepository;
        //private IHttpContextAccessor _httpContextAccessor;
        private UserManager _userManager;

        public Setup(CMDbContext dbContext, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
           // _userRepository = new UserRepository(dbContext, appSettings);
            _userManager = new UserManager(new UserRepository(dbContext), appSettings.Value.Secret);
            //_httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Will create an Admin account if none exist. Calling programs should check the .Data property, if null, instance is already configured.
        /// If non-null, get the login and password from the data structure
        /// </summary>
        /// <returns>The get.</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<APIReply> Get()
        {

            var reply = new APIReply();

           
            if(_userManager.GetUserCountAsync().Result > 0)
            {
                reply.Status = APIReply.OK;
                reply.StatusMessage = "This instance has already been configured.";
            }
            else
            {
                reply.Status = APIReply.OK;

                var password = RandomString(12);
                var user = _userManager.GenerateDefaultUser(password);

                reply.Data = new { login = user.Username,
                password = password,
                note = "Admin account has been created. Copy this password and use to connect to the application" };
            }

            return Ok(reply);

        }


        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789[];/()^~+-{}_abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
