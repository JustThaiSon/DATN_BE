namespace DATN_Models.DAL.Movie
{
    public class ActorDAL
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string Photo { get; set; }
        public int Status { get; set; }
        public Guid MovieId { get; set; }  // This is important for mapping
    }
}
