namespace DATN_Models.Models
{
    public class MembershipBenefit
    {
        public long Id { get; set; }
        public long MembershipId { get; set; }
        public string BenefitType { get; set; }
        public string ConfigJson { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
    }
}
