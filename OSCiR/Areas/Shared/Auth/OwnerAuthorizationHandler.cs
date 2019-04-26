using System.Threading.Tasks;
using OSCiR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using OSCiR.Shared;

namespace OSCiR.Areas.Shared.Auth
{
    public class OwnerAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, OwnerEntity>
    {
        public OwnerAuthorizationHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, OwnerEntity resource)
        {
            if (requirement.Name == Operations.Read.Name)
            {
                context.Succeed(requirement);
            }
            else //assume Create, Update, or Delete
            {
                if (context.User.IsInRole(UserRoles.Admin)) context.Succeed(requirement);
            }


            return Task.CompletedTask;
        }
    }
}
