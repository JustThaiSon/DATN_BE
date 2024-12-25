using System.Diagnostics.Contracts;

namespace DATN_Models.DTOS.Movies.Req
{
    public class MovieReq
    {
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Status { get; set; }
        public List<Guid>? ListActorID { get; set; }
    }
}
