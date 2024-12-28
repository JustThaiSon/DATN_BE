namespace DATN_Models.DTOS.Movies.Req.Movie
{
    public class AddMembershipReq
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long DiscountPercentage { get; set; }
        public long MonthlyFee { get; set; }
        public long DurationMonths { get; set; }
    }
}
