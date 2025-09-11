using System.Security.Claims;

namespace ProjectManager.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id ?? throw new UnauthorizedAccessException("Invalid user claim"));
        }
    }
}