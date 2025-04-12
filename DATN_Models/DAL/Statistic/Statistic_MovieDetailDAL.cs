namespace DATN_Models.DAL.Statistic
{
    public class Statistic_MovieDetailDAL
    {
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string Banner { get; set; }
        public decimal TotalRevenue { get; set; }
        public long TotalTickets { get; set; }
    }
}
