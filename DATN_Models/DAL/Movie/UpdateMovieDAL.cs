

namespace DATN_Models.DAL.Movie
{
    public class UpdateMovieDAL
    {
        public Guid MovieID { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string? BannerURL { get; set; }
        public string? ThumbnailURL { get; set; }
        public string? TrailerURL { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Status { get; set; }
        public List<Guid>? ListActorID { get; set; }
        public List<Guid>? ListGenreID { get; set; }
        public List<Guid>? ListFormatID { get; set; }
        public Guid? AgeRatingId { get; set; }
    }
}
