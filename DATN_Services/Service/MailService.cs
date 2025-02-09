using DATN_Services.Service.Interfaces;
using System.Net.Mail;
using System.Net;
using QRCoder;
using MimeKit;
using System.Drawing;
using System.Drawing.Imaging;

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

        public async Task<bool> SendQrCodeEmail(string email, string movieName, string ticketCode)
        {
            try
            {
                string qrText = $"Phim: {movieName}\nMã vé: {ticketCode}";
                byte[] qrCodeImage = GenerateQrCode(qrText);

                using (MemoryStream ms = new MemoryStream(qrCodeImage))
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.To.Add(email.Trim());
                        mail.From = new MailAddress("thaothaobatbai123@gmail.com");
                        mail.Subject = $"Vé xem phim: {movieName}";
                        mail.IsBodyHtml = true;
                        mail.Body = $"Cảm ơn bạn đã đặt vé xem phim <b>{movieName}</b>.<br/> Mã vé của bạn: <b>{ticketCode}</b><br/> Quét QR để check-in:";

                        mail.Attachments.Add(new Attachment(ms, "qrcode.png", "image/png"));

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.EnableSsl = true;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential("thaothaobatbai123@gmail.com", "kaefdapftqcriiwj");

                            await smtp.SendMailAsync(mail);
                        }
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
        private byte[] GenerateQrCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            using (Bitmap bitmap = qrCode.GetGraphic(10))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

    }
}
