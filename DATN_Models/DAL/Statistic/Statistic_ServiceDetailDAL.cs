﻿namespace DATN_Models.DAL.Statistic
{
    public class Statistic_ServiceDetailDAL
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ImageUrl { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalSold { get; set; }
    }
}
