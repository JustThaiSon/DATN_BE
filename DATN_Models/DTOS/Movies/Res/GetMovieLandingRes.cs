namespace DATN_Models.DTOS.Movies.Res
{
    public class GetMovieLandingRes
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Banner { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Rate { get; set; }
    }
}
