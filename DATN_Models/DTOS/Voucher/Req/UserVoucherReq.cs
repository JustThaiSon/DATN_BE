﻿using System;

namespace DATN_Models.DTOS.Voucher.Req
{
    public class ClaimVoucherReq
    {
        public Guid VoucherId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; } = 1; // Mặc định nhận 1 voucher
    }

    public class UpdateUserVoucherStatusReq
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }


}
