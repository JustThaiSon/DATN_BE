namespace DATN_Models.DAL.Movie
{
    public class AddMovieDAL
    {
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Banner { get; set; }
        public string Thumbnail { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Status { get; set; }
        public List<Guid>? ListActorID { get; set; }
    }
}
