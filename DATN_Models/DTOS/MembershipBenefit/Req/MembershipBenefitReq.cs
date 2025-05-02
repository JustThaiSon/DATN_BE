﻿using Microsoft.AspNetCore.Http;

namespace DATN_Models.DTOS.MembershipBenefit.Req
{
    public class MembershipBenefitReq
    {
        public long MembershipId { get; set; }
        public string BenefitType { get; set; }
        public string Description { get; set; }

        // ConfigJson sẽ được tạo dựa trên BenefitType và các trường dưới đây

        // Cho Discount - Đã bỏ ràng buộc bắt buộc
        public string? Target { get; set; }
        public decimal? Value { get; set; }

        // Cho PointBonus - Đã bỏ ràng buộc bắt buộc
        public decimal? Multiplier { get; set; }

        // Cho Service - Đã bỏ ràng buộc bắt buộc
        public Guid? ServiceId { get; set; }
        public int? Quantity { get; set; }
        public int? Limit { get; set; }

        // Cho UsePoint - Đã bỏ ràng buộc bắt buộc
        public decimal? UsePointValue { get; set; }
    }
}
