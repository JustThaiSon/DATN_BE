using Microsoft.AspNetCore.Http;

namespace DATN_Models.DTOS.Actor
{
    public class AddActorReq
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public IFormFile? Photo { get; set; }
        // Status mặc định là 1, không cần trong request
    }
}
