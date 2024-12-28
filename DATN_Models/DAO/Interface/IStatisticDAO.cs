using DATN_Models.DAL.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface IStatisticDAO
    {
        List<Statistic_SummaryDetailDAL> Summary_DateRange(DateTime? start_date, DateTime? end_date, out int response);

        // Cái này bao gồm cả số lượng thông tin vé bán ra theo ngày luôn rồi
        List<Statistic_MovieDetailDAL> Movie_DateRange(Guid MovieID, DateTime? start_date, DateTime? end_date, out int response);



        #region phụ
        Task Summary(out int response);

        Task Movie(out int response);

        #endregion

    }
}
