namespace DATN_Models.DTOS.Genre.Req
{
    public class AddGenreReq
    {
        public string GenreName { get; set; }
        public int Status { get; set; } = 1; // Default active
    }


    public class UpdateGenreReq
    {
        public string GenreName { get; set; }
        public int Status { get; set; }
    }

}