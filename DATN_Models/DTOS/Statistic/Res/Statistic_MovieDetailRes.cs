namespace DATN_Models.DTOS.Statistic.Res
{
    public class Statistic_MovieDetailRes
    {
        public DateTime Date { get; set; }
        public string MovieName { get; set; }
        public string Banner { get; set; }
        public long TotalRevenue { get; set; }
        public long TotalTickets { get; set; }
        public long TotalServices { get; set; }
    }
}
