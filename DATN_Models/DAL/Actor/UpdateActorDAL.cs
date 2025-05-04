namespace DATN_Models.DAL.Movie.Actor
{
    public class UpdateActorDAL
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string PhotoURL { get; set; } = string.Empty;
        // Status không được phép cập nhật
    }
}
