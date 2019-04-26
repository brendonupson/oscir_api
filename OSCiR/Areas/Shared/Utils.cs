using System;
using System.Security.Claims;
using OSCiR.Model;

namespace OSCiR.Areas.Shared
{
    public class Utils
    {

        internal static string getCurrentUserName(ClaimsPrincipal user)
        {
            var userName = BaseEntity.ANONYMOUS_USER;
            if (user!=null && user.Identity.IsAuthenticated)
            {
                userName = user.FindFirst(ClaimTypes.Name).Value;
            }
            return userName;
        }
    }
}
