﻿using System;

namespace DATN_Models.DAL.Voucher
{
    public class VoucherUIDAL
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
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public int ClaimedCount { get; set; }
        public int MaxClaimCount { get; set; }
        public bool IsStackable { get; set; }
        public int VoucherType { get; set; }
    }
}
