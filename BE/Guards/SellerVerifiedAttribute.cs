using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BE.Guards;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SellerVerifiedAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user == null || !user.Identity?.IsAuthenticated == true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var isVerifiedClaim = user.FindFirst("verified")?.Value;
        var roleClaim = user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        if (roleClaim != "Seller" || isVerifiedClaim != "true")
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
