namespace DATN_Models.DAL.Movie
{
    public class GetDetailMovieLangdingDAL
    {
        public Guid Id { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Banner { get; set; }
        public string GenreList { get; set; }
        public string ActorList { get; set; }
        public List<MovieGenreDAL>? genres { get; set; }
        public List<ListActorLangdingDAL>? Actors { get; set; }
    }
}
