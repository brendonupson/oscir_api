using System.Threading.Tasks;
using OSCiR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using OSCiR.Shared;

namespace OSCiR.Areas.Shared.Auth
{
    public class ClassAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, ClassEntity>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ClassEntity resource)
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

    public class ClassRelationshipAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, ClassRelationshipEntity>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ClassRelationshipEntity resource)
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


    public class ClassPropertyAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, ClassPropertyEntity>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ClassPropertyEntity resource)
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

    public class ClassExtendAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, ClassExtendEntity>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ClassExtendEntity resource)
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
