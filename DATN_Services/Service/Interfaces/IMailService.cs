using DATN_Models.DAL.Orders;

namespace DATN_Services.Service.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMail(string email, string subject, string body);

        Task<bool> SendQrCodeEmail(OrderMailResultDAL req);
    }
}
