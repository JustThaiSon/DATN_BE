namespace DATN_Models.DTOS.Movies.Res
{
    public class GetDetailMovieLangdingRes
    {
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Banner { get; set; }
        public List<ListGenreRes> Genres { get; set; }
        public List<ListActorRes> Actors { get; set; }
    }
}
