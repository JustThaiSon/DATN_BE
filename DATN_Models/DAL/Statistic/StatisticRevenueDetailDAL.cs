﻿using System;

namespace DATN_Models.DAL.Statistic
{
    public class StatisticRevenueDetailDAL
    {
        public Guid? CinemasID { get; set; }
        public string CinemaName { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalTickets { get; set; }
        public int TotalServices { get; set; }
    }
}
