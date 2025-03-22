using System;

namespace DATN_Models.Models
{
    public class VoucherUsage
    {
        public Guid Id { get; set; }
        public Guid VoucherId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrderId { get; set; }
        public DateTime UsedAt { get; set; }
        public int Status { get; set; } // 1: Used, 2: Canceled, etc.
    }
}