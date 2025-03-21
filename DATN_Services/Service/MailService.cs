using DATN_Models.DAL.Orders;
using DATN_Services.Service.Interfaces;
using QRCoder;
using System.Net;
using System.Net.Mail;

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

        public async Task<bool> SendQrCodeEmail(OrderMailResultDAL req)
        {
            try
            {


                // 1️⃣ Tạo mã QR
                string qrText = $"Phim: {req.MovieName}\nMã vé: {req.OrderCode}";
                byte[] qrCodeImage = GenerateQrCode(qrText);

                // 2️⃣ Tạo nội dung HTML
                string emailBody = $@"
      <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Template</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        .container {{
            margin: 0 auto;
            padding: 20px;
            max-width: 600px;
            border: 1px solid #ddd;
            border-radius: 8px;
        }}
        .header {{
            text-align: center;
        }}
        .header img {{
            max-width: 150px;
        }}
        .section {{
            margin-top: 20px;
        }}
        .qr-code {{
            text-align: center;
            margin: 20px 0;
        }}
        .info {{
            margin-top: 10px;
        }}
        .info table {{
            width: 100%;
            border-collapse: collapse;
        }}
        .info table td {{
            padding: 10px;
            border: 1px solid #ddd;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 0.9em;
            color: #555;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <img src=""https://banner2.cleanpng.com/20181203/orv/kisspng-cj-cgv-vietnam-cinema-cj-group-film-1713914319903.webp"" alt=""Logo"">
            <h2>{req.MovieName}</h2>
            <p><strong>{req.CinemaName}</strong></p>
            <p>{req.Address}</p>
        </div>
        <div class=""qr-code"">
            <h3>Mã Vé (Reservation Code)</h3>
            <h2>{req.OrderCode}</h2>
            <img src='cid:qrcode' alt='QR Code' width='150' height='150'>
        </div>
        <div class=""section"">
            <h3>Suất Chiếu (Session)</h3>
            <p><strong>{req.SessionTime}</strong></p>
            <p>
                Quý khách vui lòng tới quầy dịch vụ xuất trình mã vé này để được nhận vé.<br>
                <em>Please go to the service counter and present your booking code to receive the physical ticket to check-in.</em>
            </p>
        </div>
        <div class=""info"">
            <table>
                <tr>
                    <td>Phòng Chiếu (Hall)</td>
                    <td>{req.RoomName}</td>
                </tr>
                <tr>
                    <td>Ghế (Seat)</td>
                    <td>{req.SeatList}</td>
                </tr>
                <tr>
                    <td>Thời Gian Thanh Toán (Payment Time)</td>
                    <td>{req.CreatedDate}</td>
                </tr>
                <tr>
                    <td>Tiền combo bỏng nước (Concession amount)</td>
                    <td>{req.ConcessionAmount} VND</td>
                </tr>
                <tr>
                    <td>Tổng Tiền (Total amount)</td>
                    <td>{req.TotalPrice} VND</td>
                </tr>
                <tr>
                    <td>Số tiền giảm giá (Discount amount)</td>
                    <td>{req.TotalPrice} VND</td>
                </tr>
                <tr>
                    <td>Số tiền thanh toán (Payment amount)</td>
                    <td>{req.TotalPrice} VND</td>
                </tr>
            </table>
        </div>
        <div class=""footer"">
            <p>Cảm ơn quý khách đã sử dụng dịch vụ của chúng tôi!</p>
        </div>
    </div>
</body>
</html>
";

                // 3️⃣ Tạo email
                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(req.Email.Trim());
                    mail.From = new MailAddress("thaothaobatbai123@gmail.com");
                    mail.Subject = $"Vé xem phim: {req.MovieName}";
                    mail.IsBodyHtml = true;

                    // 4️⃣ Nhúng mã QR vào email
                    using (MemoryStream ms = new MemoryStream(qrCodeImage))
                    {
                        LinkedResource qrResource = new LinkedResource(ms, "image/png")
                        {
                            ContentId = "qrcode",
                            TransferEncoding = System.Net.Mime.TransferEncoding.Base64
                        };

                        // 5️⃣ Tạo nội dung HTML với hình ảnh nhúng
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailBody, null, "text/html");
                        htmlView.LinkedResources.Add(qrResource);
                        mail.AlternateViews.Add(htmlView);

                        // 6️⃣ Cấu hình SMTP
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
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text cannot be null or empty.");
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(10); // Thay đổi độ phân giải nếu cần
        }


    }
}


