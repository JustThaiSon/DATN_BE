namespace DATN_Models.DTOS.Movies.Req.Movie
{
    public class UpdateMembershipReq
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long DiscountPercentage { get; set; }
        public long MonthlyFee { get; set; }
        public long DurationMonths { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
    }
}
