namespace DATN_Models.DAL.Movie
{
   public class GetMovieByShowTimeDAL
    {
        public string Thumbnail { get; set; }
        public string MovieName { get; set; }
        public string CinemaName { get; set; }
        public DateTime StartTime { get; set; }
        public string StartTimeFormatted { get; set; }
        public string DurationFormatted { get; set; }
    }
}
