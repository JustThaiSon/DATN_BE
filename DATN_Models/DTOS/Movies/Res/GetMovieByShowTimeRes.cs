namespace DATN_Models.DTOS.Movies.Res
{
   public class GetMovieByShowTimeRes
    {
        public string Thumbnail { get; set; }
        public string MovieName { get; set; }
        public string CinemaName { get; set; }
        public DateTime StartTime { get; set; }
        public string StartTimeFormatted { get; set; }
        public string DurationFormatted { get; set; }
        public double? AverageRating { get; set; }
        public string RoomTypeName { get; set; }
    }
}
