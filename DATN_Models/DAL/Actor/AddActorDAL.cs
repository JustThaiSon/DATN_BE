namespace DATN_Models.DAL.Movie.Actor
{
    public class AddActorDAL
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string PhotoURL { get; set; } = "https://images.squarespace-cdn.com/content/v1/5cf0d08d5fc69d000172462a/1693729513955-IDRE7K0LBYSP02OO6CV2/Joe+Street+Actor+Headshot.jpg";
        public int Status { get; set; }
    }
}
