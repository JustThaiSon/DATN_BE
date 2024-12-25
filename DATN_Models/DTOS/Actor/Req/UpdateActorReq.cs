using Microsoft.AspNetCore.Http;

namespace DATN_Models.DTOS.Actor
{
    public class UpdateActorReq
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public IFormFile? Photo { get; set; }
        public int Status { get; set; }
    }
}
