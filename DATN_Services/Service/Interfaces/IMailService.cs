namespace DATN_Services.Service.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMail(string email, string subject, string body);

        Task<bool> SendQrCodeEmail(
             string email, string movieName, string ticketCode, string cinemaName, string cinemaAddress,
             string sessionTime, string hall, string seatList);
    }
}
