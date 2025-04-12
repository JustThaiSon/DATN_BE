using DATN_Models.DAL.Statistic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface IStatisticDAO
    {
        List<StatisticTopServicesDAL> GetTopServices(DateTime? startDate, DateTime? endDate, out int response);
        List<StatisticSeatProfitabilityDAL> GetSeatProfitability(DateTime? startDate, DateTime? endDate, out int response);
        List<StatisticSeatOccupancyDAL> GetSeatOccupancy(DateTime? startDate, DateTime? endDate, out int response);
        List<StatisticRevenueByTimeDAL> GetRevenueByTime(DateTime? startDate, DateTime? endDate, out int response);
        List<StatisticRevenueByCinemaDAL> GetRevenueByCinema(DateTime? startDate, DateTime? endDate, out int response);
        List<StatisticPopularGenresDAL> GetPopularGenres(DateTime? startDate, DateTime? endDate, out int response);
        List<StatisticPeakHoursDAL> GetPeakHours(DateTime? startDate, DateTime? endDate, out int response);
        List<StatisticCustomerGenderDAL> GetCustomerGender(DateTime? startDate, DateTime? endDate, out int response);
        List<StatisticBundledServicesDAL> GetBundledServices(DateTime? startDate, DateTime? endDate, out int response);






        List<Statistic_SummaryDetailDAL> Summary_DateRange(DateTime? start_date, DateTime? end_date, out int response);
        List<Statistic_SummaryDetailDAL> Summary_DateRange_Detail(DateTime? start_date, DateTime? end_date, out int response);
        List<StatisticRevenueDetailDAL> GetRevenueDetail(Guid? cinemasId, DateTime? start_date, DateTime? end_date, out int response);
        List<Statistic_MovieDetailDAL> Movie_DateRange(DateTime? start_date, DateTime? end_date, out int response);



        #region phụ
        Task Summary(out int response);

        Task Movie(out int response);

        #endregion

    }
}

