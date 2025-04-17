using System;

namespace DATN_Models.DTOS.Voucher.Res
{
    public class VoucherUsageRes
    {
        public Guid Id { get; set; }
        public Guid VoucherId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrderId { get; set; }
        public DateTime UsedAt { get; set; }
        public int Status { get; set; }

        // Thông tin bổ sung
        public string VoucherCode { get; set; }
        public string UserName { get; set; }
        public string OrderCode { get; set; }
    }

    public class VoucherRes
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public int Status { get; set; }
        public bool IsStackable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}