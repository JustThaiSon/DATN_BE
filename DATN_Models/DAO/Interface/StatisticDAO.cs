using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Statistic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO.Interface
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

        #region Statistic_Nghia
        public List<Statistic_SummaryDetailDAL> Summary_DateRange(
            DateTime? start_date,
            DateTime? end_date,
            out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[3];

                // ko nhập thì truyền null vào thủ tục thôi (nghĩa code trong SP rồi)
                pars[0] = new SqlParameter("@_StartDate", start_date.HasValue ? (object)start_date.Value : DBNull.Value);
                pars[1] = new SqlParameter("@_EndDate", end_date.HasValue ? (object)end_date.Value : DBNull.Value);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.GetListSP<Statistic_SummaryDetailDAL>("SP_Statistic_SummaryDetail_DateRange", pars);

                response = ConvertUtil.ToInt(pars[2].Value);

                return result;

            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }


        public List<Statistic_MovieDetailDAL> Movie_DateRange(
            Guid MovieID,
            DateTime? start_date,
            DateTime? end_date,
            out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];

                // ko nhập thì truyền null vào thủ tục thôi (nghĩa code trong SP rồi)
                pars[0] = new SqlParameter("@_MovieId", MovieID);
                pars[1] = new SqlParameter("@_StartDate", start_date.HasValue ? (object)start_date.Value : DBNull.Value);
                pars[2] = new SqlParameter("@_EndDate", end_date.HasValue ? (object)end_date.Value : DBNull.Value);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.GetListSP<Statistic_MovieDetailDAL>("SP_Statistic_MovieDetail_DateRange", pars);

                response = ConvertUtil.ToInt(pars[3].Value);

                return result;

            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }


        public Task Movie(out int response) { throw new NotImplementedException(); }


        public Task Summary(out int response) { throw new NotImplementedException(); }

        public List<StatisticTopServicesDAL> GetTopServices(DateTime? startDate, DateTime? endDate, out int response)
        {
            throw new NotImplementedException();
        }

        public List<StatisticSeatProfitabilityDAL> GetSeatProfitability(DateTime? startDate, DateTime? endDate, out int response)
        {
            throw new NotImplementedException();
        }

        public List<StatisticSeatOccupancyDAL> GetSeatOccupancy(DateTime? startDate, DateTime? endDate, out int response)
        {
            throw new NotImplementedException();
        }

        public List<StatisticRevenueByTimeDAL> GetRevenueByTime(DateTime? startDate, DateTime? endDate, out int response)
        {
            throw new NotImplementedException();
        }

        public List<StatisticRevenueByCinemaDAL> GetRevenueByCinema(DateTime? startDate, DateTime? endDate, out int response)
        {
            throw new NotImplementedException();
        }

        public List<StatisticPopularGenresDAL> GetPopularGenres(DateTime? startDate, DateTime? endDate, out int response)
        {
            throw new NotImplementedException();
        }

        public List<StatisticPeakHoursDAL> GetPeakHours(DateTime? startDate, DateTime? endDate, out int response)
        {
            throw new NotImplementedException();
        }

        public List<StatisticCustomerGenderDAL> GetCustomerGender(DateTime? startDate, DateTime? endDate, out int response)
        {
            throw new NotImplementedException();
        }

        public List<StatisticBundledServicesDAL> GetBundledServices(DateTime? startDate, DateTime? endDate, out int response)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}