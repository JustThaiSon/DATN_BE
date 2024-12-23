using Microsoft.AspNetCore.Identity;

namespace DATN_Models.Models
{
    public class AppRoles : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
