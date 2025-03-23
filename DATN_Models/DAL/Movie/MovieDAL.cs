using DATN_Models.DAL.Rating;

namespace DATN_Models.DAL.Movie
{
    public class MovieDAL
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; }
        public int Status { get; set; }
        public List<ActorDAL>? listdienvien { get; set; }
        public List<MovieGenreDAL>? genres { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double AverageRating { get; set; } // Add this for average rating
    }
}