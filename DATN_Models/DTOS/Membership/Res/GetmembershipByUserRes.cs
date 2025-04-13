using DATN_Models.DAL.Membership;

namespace DATN_Models.DTOS.Membership.Res
{
    public class GetmembershipByUserRes
    {
        public UserMembershipDetailsRes UserMembershipDetails { get; set; }
        public List<MembershipBenefitRes> CurrentLevelBenefits { get; set; }
        public List<MembershipBenefitRes> NextLevelBenefits { get; set; }

    }

    public class UserMembershipDetailsRes
    {
        public string UserName { get; set; }
        public string MemberCode { get; set; }
        public byte[] MemberCodeBase64 { get; set; }
        public string MembershipName { get; set; }
        public decimal MembershipPrice { get; set; }
        public DateTime PurchasedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int MembershipLevel { get; set; }
    }

    public class MembershipBenefitRes
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
