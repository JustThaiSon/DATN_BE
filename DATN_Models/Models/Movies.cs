namespace DATN_Models.Models
{
    public class Movies
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Banner { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; } // thời lượng của phim tính bằng phút
        public int Status { get; set; } = 1;
        public DateTime ReleaseDate { get; set; }

        public DateTime ImportDate { get; set; } // ngày nhập phim vào hệ thống
        public DateTime EndDate { get; set; } // ngày hết hạn của phim
        public Guid? AgeRatingId { get; set; }
    }
}
