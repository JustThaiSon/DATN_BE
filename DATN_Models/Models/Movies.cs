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
        public int Duration { get; set; } // thời kuong của phim tính bằng phút
        public int Status { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
