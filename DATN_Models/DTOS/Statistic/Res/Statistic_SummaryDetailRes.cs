﻿namespace DATN_Models.DTOS.Statistic.Res
{
    public class Statistic_SummaryDetailRes
    {
        public DateTime Date { get; set; }
        public long TotalRevenue { get; set; }
        public long TotalOrders { get; set; }
        public long TotalTickets { get; set; }
        public long TotalServices { get; set; }
    }
}
