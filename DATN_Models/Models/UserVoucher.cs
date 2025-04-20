﻿using System;

namespace DATN_Models.Models
{
    public class UserVoucher
    {
        public Guid Id { get; set; }
        public Guid VoucherId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ClaimedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Status { get; set; }
        public int Quantity { get; set; }
        public int UsedQuantity { get; set; }
    }
}
