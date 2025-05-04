using Microsoft.AspNetCore.Identity;

namespace DATN_Models.Models
{
    public class AppUsers_Cinemas
    {
        public Guid UserId { get; set; }
        public Guid CinemasId { get; set; }
    }
}
