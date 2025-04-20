﻿namespace DATN_Models.DAL.MembershipBenefit
{
    public class MembershipBenefitDAL
    {
        public long Id { get; set; }
        public long MembershipId { get; set; }
        public string MembershipName { get; set; }
        public string BenefitType { get; set; }
        public string ConfigJson { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        
        // Các trường phân tích từ ConfigJson
        public string Target { get; set; }
        public decimal? Value { get; set; }
        public decimal? Multiplier { get; set; }
        public Guid? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int? Quantity { get; set; }
        public int? Limit { get; set; }
        public decimal? UsePointValue { get; set; }
    }
}
