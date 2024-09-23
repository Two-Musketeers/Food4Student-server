using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("user_id") ?? user.FindFirstValue("sub");

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new Exception("Cannot get user ID from token");
            }

            return userIdClaim;
        }
    }
}