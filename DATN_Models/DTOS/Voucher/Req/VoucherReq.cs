using System;

namespace DATN_Models.DTOS.Voucher.Req
{
    public class UseVoucherReq
    {
        public Guid VoucherId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrderId { get; set; }
    }


    public class VoucherReq
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MinOrderValue { get; set; } = 0; // Thêm trường MinOrderValue với giá trị mặc định là 0

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUsage { get; set; }
        public int MaxClaimCount { get; set; }
        public int Status { get; set; }
        public bool IsStackable { get; set; } = false; // Mặc định là không stack được
        public int VoucherType { get; set; } = 0; // Mặc định là 0
    }
}