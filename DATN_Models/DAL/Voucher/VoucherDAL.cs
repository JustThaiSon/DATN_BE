using System;

namespace DATN_Models.DAL.Voucher
{
    public class VoucherUsageDAL
    {
        public Guid Id { get; set; }
        public Guid VoucherId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrderId { get; set; }
        public DateTime UsedAt { get; set; }
        public int Status { get; set; }

        // Thêm các thông tin bổ sung (để hiển thị)
        public string VoucherCode { get; set; }
        public string UserName { get; set; }
        public string OrderCode { get; set; }
    }


    public class VoucherDAL
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MinOrderValue { get; set; } = 0; // Thêm trường MinOrderValue với giá trị mặc định là 0
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public int ClaimedCount { get; set; }
        public int MaxClaimCount { get; set; }
        public int Status { get; set; }
        public bool IsStackable { get; set; }
        public int VoucherType { get; set; } = 0; // Mặc định là 0
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}