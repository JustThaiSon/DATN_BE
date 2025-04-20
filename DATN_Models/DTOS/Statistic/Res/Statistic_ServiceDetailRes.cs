﻿namespace DATN_Models.DTOS.Statistic.Res
{
    public class Statistic_ServiceDetailRes
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ImageUrl { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalSold { get; set; }
    }
}
