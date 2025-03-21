using Microsoft.AspNetCore.Http;

namespace DATN_Helpers.Extensions
{
    public static class HttpContextHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static Guid GetUserId()
        {
            if (_httpContextAccessor?.HttpContext == null)
            {
                return Guid.Empty;
            }

            return (Guid)(_httpContextAccessor.HttpContext.Items["UserId"] ?? Guid.Empty);
        }

        public static List<string> GetRoles()
        {
            if (_httpContextAccessor?.HttpContext.Items["Roles"] is List<string> roles)
            {
                return roles;  // Return the list of roles
            }
            return new List<string>();
        }
    }
}
