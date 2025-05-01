using DATN_Models.DAL.Orders;
using DATN_Models.DTOS.Order.Res;

namespace DATN_Services.Service.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMail(string email, string subject, string body);

        Task<bool> SendQrCodeEmail(OrderMailResultRes req);
        byte[] GenerateQrCode(string text);
        Task<bool> SendMailRefund(GetInfoRefundRes req);
        Task<bool> SendOtpEmail(string email, string optCode);
        Task<bool> SendMailRefundAll(GetInfoRefundRes req);
    }
}
