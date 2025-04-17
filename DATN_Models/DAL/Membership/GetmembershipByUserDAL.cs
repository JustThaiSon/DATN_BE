namespace DATN_Models.DAL.Membership
{
    public class GetmembershipByUserDAL
    {
        public string RawUserMembershipDetails { get; set; }
        public string RawCurrentLevelBenefits { get; set; }
        public string RawNextLevelBenefits { get; set; }

        public UserMembershipDetailsDAL UserMembershipDetails { get; set; }
        public List<MembershipBenefitDAL> CurrentLevelBenefits { get; set; }
        public List<MembershipBenefitDAL> NextLevelBenefits { get; set; }
    }

    public class UserMembershipDetailsDAL
    {
        public string UserName { get; set; }
        public string MemberCode { get; set; }
        public string MembershipName { get; set; }
        public decimal MembershipPrice { get; set; }
        public decimal MembershipPriceNext { get; set; }
        public DateTime PurchasedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int MembershipLevel { get; set; }
    }

    public class MembershipBenefitDAL
    {
        public int MembershipId { get; set; }
        public string MembershipName { get; set; }
        public int MembershipLevel { get; set; }
        public int BenefitId { get; set; }
        public string BenefitDescription { get; set; }
        public string LogoUrl { get; set; }
        public int? IsCurrentLevelBenefit { get; set; }
        public int? IsNextLevelBenefit { get; set; }
    }
}
