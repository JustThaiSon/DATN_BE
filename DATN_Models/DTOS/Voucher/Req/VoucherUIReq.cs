﻿using System;
using Microsoft.AspNetCore.Http;

namespace DATN_Models.DTOS.Voucher.Req
{
    public class VoucherUIReq
    {
        public Guid VoucherId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? Photo { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; } = 1; // Mặc định là active
    }
}
