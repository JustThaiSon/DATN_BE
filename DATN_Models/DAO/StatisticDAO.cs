using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Statistic;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO
{
    public class StatisticDAO : IStatisticDAO
    {
        private static string connectionString = string.Empty;

        public StatisticDAO()
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public List<StatisticTopServicesDAL> GetTopServices(DateTime? startDate, DateTime? endDate, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<StatisticTopServicesDAL>("SP_Statistic_TopServices", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<StatisticTopServicesDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting top services statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<StatisticSeatProfitabilityDAL> GetSeatProfitability(DateTime? startDate, DateTime? endDate, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<StatisticSeatProfitabilityDAL>("SP_Statistic_SeatProfitability", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<StatisticSeatProfitabilityDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting seat profitability statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<StatisticSeatOccupancyDAL> GetSeatOccupancy(DateTime? startDate, DateTime? endDate, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<StatisticSeatOccupancyDAL>("SP_Statistic_SeatOccupancy", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<StatisticSeatOccupancyDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting seat occupancy statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<StatisticRevenueByTimeDAL> GetRevenueByTime(DateTime? startDate, DateTime? endDate, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_StartDate", startDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@_EndDate", endDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<StatisticRevenueByTimeDAL>("SP_Statistic_RevenueByTime", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<StatisticRevenueByTimeDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting revenue by time statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<StatisticRevenueByCinemaDAL> GetRevenueByCinema(DateTime? startDate, DateTime? endDate, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_StartDate", startDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@_EndDate", endDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<StatisticRevenueByCinemaDAL>("SP_Statistic_RevenueByCinema", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<StatisticRevenueByCinemaDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting revenue by cinema statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<StatisticPopularGenresDAL> GetPopularGenres(DateTime? startDate, DateTime? endDate, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<StatisticPopularGenresDAL>("SP_Statistic_PopularGenres", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<StatisticPopularGenresDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting popular genres statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<StatisticPeakHoursDAL> GetPeakHours(DateTime? startDate, DateTime? endDate, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<StatisticPeakHoursDAL>("SP_Statistic_PeakHours", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<StatisticPeakHoursDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting peak hours statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<StatisticCustomerGenderDAL> GetCustomerGender(DateTime? startDate, DateTime? endDate, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<StatisticCustomerGenderDAL>("SP_Statistic_CustomerGender", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<StatisticCustomerGenderDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting customer gender statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<StatisticBundledServicesDAL> GetBundledServices(DateTime? startDate, DateTime? endDate, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<StatisticBundledServicesDAL>("SP_Statistic_BundledServices", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<StatisticBundledServicesDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting bundled services statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<Statistic_SummaryDetailDAL> Summary_DateRange(DateTime? start_date, DateTime? end_date, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_StartDate", start_date ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@_EndDate", end_date ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<Statistic_SummaryDetailDAL>("SP_Statistic_Summary_DateRange", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<Statistic_SummaryDetailDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting bundled services statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<Statistic_MovieDetailDAL> Movie_DateRange(DateTime? start_date, DateTime? end_date, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_StartDate", start_date ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@_EndDate", end_date ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<Statistic_MovieDetailDAL>("SP_Statistic_Movie", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result ?? new List<Statistic_MovieDetailDAL>();
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error getting bundled services statistics", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public Task Summary(out int response)
        {
            throw new NotImplementedException();
        }

        public Task Movie(out int response)
        {
            throw new NotImplementedException();
        }
    }
}
