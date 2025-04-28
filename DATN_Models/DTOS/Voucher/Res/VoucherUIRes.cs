﻿using System;

namespace DATN_Models.DTOS.Voucher.Res
{
    public class VoucherUIRes
    {
        public Guid Id { get; set; }
        public Guid VoucherId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Thông tin bổ sung về voucher
        public string VoucherCode { get; set; }
        public string VoucherDescription { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MinOrderValue { get; set; } // Thêm trường MinOrderValue
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public int ClaimedCount { get; set; }
        public int MaxClaimCount { get; set; }
        public bool IsStackable { get; set; }
        public int VoucherType { get; set; }
    }
}
