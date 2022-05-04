using System.Linq;
using System.Security.Claims;

namespace ComputerHouse.Extensions
{
    public static class UserIdentityExtensions
    {
        public static string GetUserIdByClaimsPrincipal(this ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        }

        public static string GetUserEmailByClaimsPrincipal(this ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
    }
}
