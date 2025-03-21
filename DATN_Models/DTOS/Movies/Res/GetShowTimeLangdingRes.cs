namespace DATN_Models.DTOS.Movies.Res
{
    public class GetShowTimeLangdingRes
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<ShowtimesLangdingRes> Showtimes { get; set; }
    }
}

