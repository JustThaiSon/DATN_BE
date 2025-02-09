namespace DATN_Services.Service.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMail(string email, string subject, string body);
    }
}
