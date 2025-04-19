﻿using System;

namespace DATN_Models.DTOS.Voucher.Res
{
    public class UserVoucherRes
    {
        public Guid Id { get; set; }
        public Guid VoucherId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ClaimedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Status { get; set; }
        public int Quantity { get; set; }
        public int UsedQuantity { get; set; }
        public int RemainingQuantity { get { return Quantity - UsedQuantity; } }

        // Thông tin bổ sung
        public string VoucherCode { get; set; }
        public string VoucherDescription { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }

    public class AvailableVoucherRes
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
        public int ClaimedCount { get; set; }
        public int MaxClaimCount { get; set; }
        public int Status { get; set; }
        public bool IsStackable { get; set; }
        public int RemainingClaims { get { return MaxClaimCount - ClaimedCount; } }
    }
}
