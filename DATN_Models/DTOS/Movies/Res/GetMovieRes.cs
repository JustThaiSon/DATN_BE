using DATN_Models.DAL.Movie;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.Models;

namespace DATN_Models.DTOS.Movies.Res
{
    public class GetMovieRes
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; }
        public int Status { get; set; }
        public List<ActorDAL>? listdienvien { get; set; }
        public List<MovieGenreRes>? genres { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double AverageRating { get; set; } // Add this

    }
}


