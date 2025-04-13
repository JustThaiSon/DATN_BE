using DATN_Models.DAL.Counter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface ICounterDAO
    {
        List<Counter_NowPlayingMovies_GetList_DAL> GetNowPlayingMovies(
           int currentPage,
           int recordPerPage,
           DateTime? showDate,
           Guid? cinemaId,
           Guid? genreId,
           out int totalRecord,
           out int response);

        // Phương thức lấy thông tin vé từ QR code
        DataSet GetTicketInfoFromQR(
            string orderCode,
            out int response,
            out string message);

        // Phương thức xác nhận sử dụng vé
        bool ConfirmTicketUsage(
            string orderCode,
            out int response,
            out string message);
    }
}
