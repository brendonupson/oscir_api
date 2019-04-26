using System;
using System.Threading.Tasks;
using OSCiR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using OSCiR.Shared;

namespace OSCiR.Areas.Shared.Auth
{
    public class UserAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, UserEntity>
    {
        public UserAuthorizationHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, UserEntity resource)
        {
            /*if (requirement.Name == Operations.Read.Name)
            {
                context.Succeed(requirement);
            }*/

            //Only allow admins access
            if (context.User.IsInRole(UserRoles.Admin)) context.Succeed(requirement);



            return Task.CompletedTask;
        }
    }
}
