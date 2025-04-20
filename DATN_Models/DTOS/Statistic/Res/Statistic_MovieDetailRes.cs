namespace DATN_Models.DTOS.Statistic.Res
{
    public class Statistic_MovieDetailRes
    {
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string Banner { get; set; }
        public decimal TotalRevenue { get; set; }
        public long TotalTickets { get; set; }
    }
}
