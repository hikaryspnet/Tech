using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Tech.API.Common
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int? UserId => int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out var result) ? result : null;
        protected string? Email => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        protected int? CompanyId => GetIntClaim("companyid");

        protected bool? IsAdmin
        {
            get
            {
                var claimValue = User.FindFirst("IsAdmin")?.Value;
                if (claimValue == null)
                    return null;

                return bool.TryParse(claimValue, out bool result) ? result : null;
            }
        }

        private int? GetIntClaim(string claimType)
        {
            var claimValue = User.FindFirst(claimType)?.Value;
            if(int.TryParse(claimValue, out int intValue)) return intValue;
            return null;
        }
    }
}
