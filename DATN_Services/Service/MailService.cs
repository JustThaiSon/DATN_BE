using DATN_Services.Service.Interfaces;
using System.Net.Mail;
using System.Net;

namespace DATN_Services.Service
{
    public class MailService : IMailService
    {
        public async Task<bool> SendMail(string email, string subject, string body)
        {

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(email.Trim());
                    mail.From = new MailAddress("thaothaobatbai123@gmail.com");
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.Body = body;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("thaothaobatbai123@gmail.com", "kaefdapftqcriiwj");

                        await smtp.SendMailAsync(mail); 
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                return false;
            }
        }
    }
}
