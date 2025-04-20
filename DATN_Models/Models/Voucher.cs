using System;

namespace DATN_Models.Models
{
    public class Voucher
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; } // "PERCENT" hoặc "FIXED"
        public decimal DiscountValue { get; set; }
        public decimal MinOrderValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public int ClaimedCount { get; set; }
        public int MaxClaimCount { get; set; }
        public int Status { get; set; }
        public bool IsStackable { get; set; }
        public int VoucherType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}