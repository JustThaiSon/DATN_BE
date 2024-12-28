namespace DATN_Models.DAL.Membership
{
    public class MembershipDAL
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
