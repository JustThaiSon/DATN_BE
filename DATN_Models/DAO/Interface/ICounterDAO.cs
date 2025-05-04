using DATN_Models.DAL.Counter;
using DATN_Models.Models;
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

        // Phương thức xử lý đơn hàng dịch vụ (thay thế các phương thức cũ)
        DataSet ManageServiceOrder(
            int action, // 1: Tạo, 2: Thanh toán, 3: Sử dụng, 4: Hoàn tất
            string orderCode,
            string email,
            Guid? userId, // ID nhân viên thực hiện giao dịch
            bool isAnonymous,
            string serviceListJson,
            out int response,
            out string message,
            out string orderCodeOut);

        // Phương thức tạo đơn hàng dịch vụ mới
        DataSet CreateServiceOrder(
            string email,
            Guid? userId,
            bool isAnonymous,
            string serviceListJson,
            out int response,
            out string message,
            out string orderCodeOut);

        // Phương thức xác nhận thanh toán đơn hàng
        DataSet ConfirmServicePayment(
            string orderCode,
            Guid? userId,
            out int response,
            out string message);

        // Phương thức xác nhận sử dụng dịch vụ
        DataSet ConfirmServiceUsage(
            string orderCode,
            Guid? userId,
            out int response,
            out string message);

        // Phương thức hoàn tất giao dịch dịch vụ
        DataSet CompleteServiceTransaction(
            string orderCode,
            Guid userId,
            out int response,
            out string message);

        // Phương thức lấy thông tin đơn hàng dịch vụ
        DataSet GetServiceOrderInfo(string orderCode);

        // Phương thức bán nhanh dịch vụ
        string QuickServiceSale(
            string serviceListJson,
            Guid userId, // ID nhân viên
            string customerEmail,
            bool markAsUsed,
            out int response,
            out string message,
            out decimal totalAmount);

        // Phương thức mới cho đặt vé kèm đồ ăn
        string CreateTicketAndServiceOrder(
            string email,
            Guid? userId,
            bool isAnonymous,
            Guid showTimeId,
            string seatListJson,
            string serviceListJson,
            out int response,
            out string message);

        // Phương thức xác nhận thanh toán đơn hàng vé
        bool ConfirmTicketAndServicePayment(
               string orderCode,
               Guid? userId,
               out int response,
               out string message);

        // Phương thức lấy thông tin đơn hàng vé
        DataSet GetTicketAndServiceOrderInfo(
            string orderCode,
            out int response,
            out string message);

        // Phương thức hủy đơn hàng chưa thanh toán
        bool CancelUnpaidOrder(
            string orderCode,
            Guid? userId,
            out int response,
            out string message);

        // Phương thức hoàn vé dựa trên OrderCode
        DataSet RefundOrderByOrderCode(
            string orderCode,
            out int response,
            out string message);

        // Phương thức lấy thông tin người dùng theo email
        AppUsers GetUserByEmail(
            string email,
            out int response,
            out string message);


        // Phương thức lấy thông tin chi tiết đơn hàng theo OrderCode
        DataSet GetOrderDetailsByOrderCode(
            string orderCode,
            out int response);


    }


}