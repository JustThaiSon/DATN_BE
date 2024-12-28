namespace DATN_Models.DAL.Membership
{
    public class AddMembershipDAL
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long DiscountPercentage { get; set; }
        public long MonthlyFee { get; set; }
        public long DurationMonths { get; set; }
    }
}
