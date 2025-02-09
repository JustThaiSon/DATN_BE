namespace DATN_Models.DAL.Movie
{
    public class GetMovieLandingDAL
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Banner { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
