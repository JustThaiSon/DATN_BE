using Microsoft.AspNetCore.Http;

namespace DATN_Models.DTOS.Movies.Req.Movie
{
    public class AddMovieReq
    {
        public string MovieName { get; set; }
        public string Description { get; set; }
        public IFormFile? Banner { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public IFormFile? Trailer { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ImportDate { get; set; } // ngày nhập phim vào hệ thống
        public DateTime EndDate { get; set; } // ngày hết hạn của phim
        public int Status { get; set; }
        public List<Guid>? ListActorID { get; set; }
        public List<Guid>? ListGenreID { get; set; }
        public Guid? AgeRatingId { get; set; }
        public List<Guid>? ListFormatID { get; set; }
    }
}
