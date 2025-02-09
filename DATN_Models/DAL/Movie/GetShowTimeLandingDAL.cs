namespace DATN_Models.DAL.Movie
{
    public class GetShowTimeLandingDAL
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ListShowTime { get; set; }
        public List<ShowtimesLangdingDAL> Showtimes { get; set; }
    }
}
