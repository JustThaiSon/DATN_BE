﻿using System;

namespace DATN_Models.DAL.Voucher
{
    public class UserVoucherDAL
    {
        public Guid Id { get; set; }
        public Guid VoucherId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ClaimedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Status { get; set; } // 0: Available, 1: Used, 2: Expired, 3: Canceled
        public int Quantity { get; set; } // Số lượng voucher mà người dùng đang có
        public int UsedQuantity { get; set; } // Số lượng voucher đã sử dụng

        // Thông tin bổ sung (để hiển thị)
        public string VoucherCode { get; set; }
        public string VoucherDescription { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public int VoucherType { get; set; }
        public decimal MinOrderValue { get; set; } // Giá trị đơn hàng tối thiểu để áp dụng voucher
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
