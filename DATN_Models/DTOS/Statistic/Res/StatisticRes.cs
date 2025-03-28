using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Statistic.Res
{
    public class StatisticRes
    {
        public class StatisticTopServicesRes
        {
            public string ServiceName { get; set; }
            public int TotalSold { get; set; }
            public decimal TotalRevenue { get; set; }
        }

        // StatisticSeatProfitabilityRes.cs
        public class StatisticSeatProfitabilityRes
        {
            public string SeatTypeName { get; set; }
            public decimal AveragePrice { get; set; }
            public int TotalTicketsSold { get; set; }
            public decimal TotalRevenue { get; set; }
        }

        // StatisticSeatOccupancyRes.cs
        public class StatisticSeatOccupancyRes
        {
            public DateTime StartTime { get; set; }
            public string MovieName { get; set; }
            public int TotalSeats { get; set; }
            public int BookedSeats { get; set; }
            public double OccupancyRate { get; set; }
        }

        // StatisticRevenueByTimeRes.cs
        public class StatisticRevenueByTimeRes
        {
            public DateTime OrderDate { get; set; }
            public int HourOfDay { get; set; }
            public decimal TotalRevenue { get; set; }
            public int TotalOrders { get; set; }
        }

        // StatisticRevenueByCinemaRes.cs
        public class StatisticRevenueByCinemaRes
        {
            public string CinemaName { get; set; }
            public decimal TotalRevenue { get; set; }
            public int TotalOrders { get; set; }
        }

        // StatisticPopularGenresRes.cs
        public class StatisticPopularGenresRes
        {
            public string GenreName { get; set; }
            public int TotalShowtimes { get; set; }
            public int TotalBookedSeats { get; set; }
        }

        // StatisticPeakHoursRes.cs
        public class StatisticPeakHoursRes
        {
            public int HourOfDay { get; set; }
            public int TotalTicketsSold { get; set; }
        }

        // StatisticCustomerGenderRes.cs
        public class StatisticCustomerGenderRes
        {
            public string Gender { get; set; }
            public int TotalCustomers { get; set; }
            public decimal TotalSpent { get; set; }
        }

        // StatisticBundledServicesRes.cs
        public class StatisticBundledServicesRes
        {
            public string ServiceName { get; set; }
            public int TotalOrders { get; set; }
            public int TotalQuantitySold { get; set; }
        }
    }
}
