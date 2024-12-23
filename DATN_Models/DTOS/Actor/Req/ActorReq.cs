namespace DATN_Models.DTOS.Actor
{
    public class ActorReq
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string Photo { get; set; }
        public int Status { get; set; }
    }
}
