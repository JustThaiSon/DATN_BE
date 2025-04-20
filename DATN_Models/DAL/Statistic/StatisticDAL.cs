using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Statistic
{
    public class StatisticTopServicesDAL
    {
        public string ServiceName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class StatisticSeatProfitabilityDAL
    {
        public string SeatTypeName { get; set; }
        public decimal AveragePrice { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class StatisticSeatOccupancyDAL
    {
        public DateTime StartTime { get; set; }
        public string MovieName { get; set; }
        public int TotalSeats { get; set; }
        public int BookedSeats { get; set; }
        public double OccupancyRate { get; set; }
    }

    public class StatisticRevenueByTimeDAL
    {
        public DateTime OrderDate { get; set; }
        public int HourOfDay { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
    }

    public class StatisticRevenueByCinemaDAL
    {
        public Guid CinemasId { get; set; }
        public string Name { get; set; }
        public int TotalRooms { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalTickets { get; set; }
    }

    public class StatisticPopularGenresDAL
    {
        public string GenreName { get; set; }
        public int TotalShowtimes { get; set; }
        public int TotalBookedSeats { get; set; }
    }

    public class StatisticPeakHoursDAL
    {
        public int HourOfDay { get; set; }
        public int TotalTicketsSold { get; set; }
    }

    public class StatisticCustomerGenderDAL
    {
        public string Gender { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class StatisticBundledServicesDAL
    {
        public string ServiceName { get; set; }
        public int TotalOrders { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
