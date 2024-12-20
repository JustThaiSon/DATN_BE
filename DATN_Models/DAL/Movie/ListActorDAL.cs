namespace DATN_Models.DAL.Movie
{
    public class ListActorDAL
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string Photo { get; set; }
        public int Status { get; set; }
    }
}
