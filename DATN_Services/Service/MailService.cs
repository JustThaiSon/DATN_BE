using DATN_Models.DAL.Orders;
using DATN_Models.DTOS.Order.Res;
using DATN_Services.Service.Interfaces;
using Org.BouncyCastle.Ocsp;
using QRCoder;
using System.Net;
using System.Net.Mail;
using System.Text;
using static QRCoder.PayloadGenerator;

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
                    mail.From = new MailAddress("thaothaobatbai123@gmail.com", "CGV Booking System");
                    mail.Sender = new MailAddress("thaothaobatbai123@gmail.com");
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.Body = body;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.SubjectEncoding = Encoding.UTF8;

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
                if (ex.InnerException != null)
                    Console.WriteLine($"Chi tiết nội tại: {ex.InnerException.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> SendQrCodeEmail(OrderMailResultDAL req)
        {
            try
            {


                // 1️⃣ Tạo mã QR
                string qrText = $"{req.OrderCode}";
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
                    <td>{req.DiscountPrice} VND</td>
                </tr>
                <tr>
                    <td>Số tiền thanh toán (Payment amount)</td>
                    <td>{req.TotalPrice + req.DiscountPrice} VND</td>
                </tr>
                <tr>
                    <td>Số điểm được cộng (Add Point)</td>
                    <td>{req.PointChange} Điểm</td>
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


        public byte[] GenerateQrCode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text cannot be null or empty.");
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(10);
        }

        public async Task<bool> SendMailRefund(GetInfoRefundRes req)
        {
            try
            {
                string email = req.Email;
                string subject = "Thông báo hoàn điểm";
                string body = $@"
       
<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Document</title>
</head>

<body>
    <div style=""padding:0;margin:0;height:100%;width:100%;font-family:Arial,'Times New Roman','Calibri'"">


        <div style=""margin:0 auto;max-width:600px;display:block;background:#f0f0f0;font-family:inherit"">
            <table cellpadding=""0"" cellspacing=""0""
                style=""padding:0;border-spacing:0;background:#f0f0f0;border:0;margin:0;text-align:center;vertical-align:middle;font-weight:500;table-layout:fixed;border-collapse:collapse;height:100%;width:100%;line-height:100%""
                width=""100%"" height=""100%"" align=""center"" valign=""middle"">
                <tbody>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td
                            style=""margin:0;padding:0;border:none;border-spacing:0;background:#f0f0f0;border-collapse:collapse;font-family:inherit"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;border-spacing:0;border:0;padding:0;width:100%;border-collapse:collapse""
                                width=""100%"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center""></td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td
                                            style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">

                                            <table cellpadding=""0"" cellspacing=""0""
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%"">
                                                <tbody>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;background-image:url(https://res.cloudinary.com/dw44ghjmu/image/upload/v1745446945/LogoEmailCinex_iuwqqe.png);background-size:cover;width:612px;height:146px;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            width=""612"" height=""146""
                                                            background=""https://res.cloudinary.com/dw44ghjmu/image/upload/v1745446945/LogoEmailCinex_iuwqqe.png""
                                                            align=""center""></td>
                                                    </tr>
                                                </tbody>
                                            </table>


                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""1""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:24px;border-collapse:collapse;font-family:inherit""
                                            height=""24"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">
                                            <table cellpadding=""0"" cellspacing=""0""
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%"">
                                                <tbody>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                                            width=""72"" height=""100%"">
                                                            <div
                                                                style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                                            </div>
                                                        </td>
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <h1
                                                                style=""font-size:32px;font-weight:500;letter-spacing:0.01em;color:#141212;text-align:center;line-height:39px;margin:0;font-family:inherit"">
                                                                Thông Báo Hoàn Điểm</h1>
                                                        </td>
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                                            width=""72"" height=""100%"">
                                                            <div
                                                                style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td colspan=""1""
                            style=""margin:0;padding:0;border:none;border-spacing:0;height:0px;border-collapse:collapse;font-family:inherit""
                            height=""0"">
                            <table
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                width=""100%""></table>
                        </td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center""></td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                width=""100%"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit""
                                            height=""64"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                            width=""72"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                            </div>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">
                                            <table cellpadding=""0"" cellspacing=""0""
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;background-color:#f9f9f9;border-collapse:collapse""
                                                width=""100%"" bgcolor=""#F9F9F9"">
                                                <tbody>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td colspan=""3""
                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit""
                                                            height=""40"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%""></table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit""
                                                            width=""38"" height=""100%"">
                                                            <div
                                                                style=""height:100%;overflow:hidden;width:38px;font-family:inherit"">
                                                            </div>
                                                        </td>
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse""
                                                                width=""100%"">
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <h2
                                                                                style=""font-size:25.63px;font-weight:700;line-height:100%;color:#333;margin:0;text-align:center;font-family:inherit"">
                                                                                Tên Người Dùng <span
                                                                                    style=""color:#999;font-family:inherit"">{req.Email}</span>
                                                                            </h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td colspan=""1""
                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:8px;border-collapse:collapse;font-family:inherit""
                                                                            height=""8"">
                                                                            <table
                                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                                width=""100%""></table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                                            align=""center"">
                                                                            <p
                                                                                style=""margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit"">
                                                                               Số điểm được hoàn: </p>
                                                                               <p
                                                                               style=""margin:0;padding:0;font-weight:700;font-size:20px;line-height:140%;letter-spacing:-0.01em;color:#080606;font-family:inherit"">
                                                                              {req.PointRefund} điểm</p>
                                                                               <p
                                                                               style=""margin:0;padding:0;font-weight:500;font-size:18px;line-height:140%;letter-spacing:-0.01em;color:#666;font-family:inherit"">
                                                                              Truy cập vào Cinex.com để xem thêm chi tiết.</p>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit""
                                                            width=""38"" height=""100%"">
                                                            <div
                                                                style=""height:100%;overflow:hidden;width:38px;font-family:inherit"">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td colspan=""3""
                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit""
                                                            height=""48"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%""></table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                            width=""72"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit""
                                            height=""48"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center""></td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:16px;text-align:center;line-height:140%;letter-spacing:-0.01em;color:#666;border-collapse:collapse""
                                width=""100%"" align=""center"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit""
                                            width=""100"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:100px;font-family:inherit"">
                                            </div>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">Nếu muốn xem điểm hiện có của mình, hãy tới trang <a
                                                href=""http://localhost:4200/""
                                                style=""color:#bd2225;text-decoration:underline"" target=""_blank""
                                               >Thông tin tài khoản</a>. Sau khi đã đăng nhập thành công.</td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit""
                                            width=""100"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:100px;font-family:inherit"">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:24px;border-collapse:collapse;font-family:inherit""
                                            height=""24"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center""></td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:16px;text-align:center;line-height:140%;letter-spacing:-0.01em;color:#666;border-collapse:collapse""
                                width=""100%"" align=""center"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit""
                                            width=""100"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:100px;font-family:inherit"">
                                            </div>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">Nếu chưa có tài khoản, hãy <a
                                                href=""https://links.riotgames.com/ls/click?upn=frHghcUMWgUZ9OUHJJkDdV7REMamLoc-2FhRJgb3oH2J-2BDt6if31d3T2o-2B6NXQ9Jhtt7c3_CyPYuvKevH6nD6CMROzBiVQzIj9ecAtAT4i5qN8rzTPS70M0Tdj-2FnTRrzaK0fly80zNHaMqliceDQg7-2FI7Nzdxh6rc4O-2FlJGR-2FXhvyLnYZ7gf9Aw3ue5CqgeIu2Y-2B0ut36SxWVjDVHZhHRcb-2BLf4oLViFWt6BP6OfO6Ss0X2cbAaBqdSpqWxghXv-2FEwS0BFTsaaUSRCvl0WX5d7QjEla3SHyQxQ1OnXg8O-2FOzcOVJo-2B7Z1395NjjJC-2FslOKr5octrndcyDoXgrabyTDWtoaWP-2BRmw1hJpxjB98geqN3E1kBvEXfZzuoWjkizDCrtSNVPMx0SWGHUejW-2B-2BoRJaob6njY3ne-2FVtHMV5UxEBk2QiER9AjzN87Cf1tetplaxwk1nhpuWA7XvZR-2F1lG1VKHSjEWPP-2BDAMUD4t2Sktq9N8SqCzoZbihLOn9KpX8cTriDAhVi6731R3-2BcmU4JogPjoXPIfUssvqmmNOYKh3ntXwVnsz1Iil3CDaIkzy99qvhWEbeHTvL4ri0v6kmTw46cX8J1e0vu0hLzrHbEPoq9FWNhdkMpJC4gTXQY8nA-2FOhHeaIeRZcobynQVisoBbDbVdamDQIrOrWEHhN8HGUWiMAKuDzSQZeBba-2F3LYI7lTvxqEFVUM1s3i3fYMpbzRittkCDMghizSCTYq4EPeBpWQxBiYJbIyqTqf8jqwRdHzbE7qyH9T6n-2FJcmAKOgQjXkilY1c7NUoI5zXI6DKGNye0yO-2F02ROhHSnZt956VdejVtb8svxQnrv7tj25WYfoEpBvg1SqHBjbT91VwRQSMexbsOvVSlrMLCxZsI91FpLUYYXlgMJjbvJNphRf0-2FOf0WlaS3DnrhksU-2B8DIHgvFOSn33Yf-2BZ7s2NLLvc7BPUuNlP6vMOBYV9I0Q5Fwu2TVsmbUNMDxzY-2BkuK3XYr-2Bx0t2UeobDWgoK6qJdL1VyVuVFRtOQ0-2FiJR7O4f2o3hjrNL2Wg8HonpXEZ4ckQW95F8X3BiBE4-3D""
                                                style=""color:#bd2225;text-decoration:underline"" target=""_blank""
                                               >tạo tài khoản mới</a> để bắt đầu sử dụng điêm cho dịch vụ của CineX.</td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit""
                                            width=""100"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:100px;font-family:inherit"">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit""
                                            height=""80"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td colspan=""1""
                            style=""margin:0;padding:0;border:none;border-spacing:0;height:0px;border-collapse:collapse;font-family:inherit""
                            height=""0"">
                            <table
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                width=""100%""></table>
                        </td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center""></td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                width=""100%"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                            width=""72"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                            </div>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">
                                            <table cellpadding=""0"" cellspacing=""0""
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:11.24px;line-height:140%;letter-spacing:-0.01em;color:#999;table-layout:fixed;border-collapse:collapse""
                                                width=""100%"">
                                                <tbody>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;display:inline-table;width:auto;border-collapse:collapse"">
                                                                <tbody>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        
                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;width:32px;border-collapse:collapse;font-family:inherit""
                                                                            width=""32"" align=""center""></td>
                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                                            align=""center""><img
                                                                                src=""https://res.cloudinary.com/dw44ghjmu/image/upload/v1745447860/e9acac58-056e-4b69-bc95-0576c455b5f4_rtybyk.png""
                                                                                style=""border:0;height:auto;line-height:100%;outline:none;text-decoration:none;display:inline-block;width:187px""
                                                                                width=""187"" class=""CToWUd""
                                                                                data-bit=""iit""></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <table cellpadding=""0"" cellspacing=""0""
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%"">
                                                                <tbody>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td colspan=""1""
                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit""
                                                                            height=""48"">
                                                                            <table
                                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                                width=""100%""></table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                                            align=""center"">
                                                                            <table cellpadding=""0"" cellspacing=""0""
                                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse""
                                                                                width=""100%"">
                                                                                <tbody>
                                                                                    <tr
                                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;height:44px;width:100%;border-collapse:collapse;font-family:inherit"">
                                                                                        <td
                                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                                        </td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""44"" height=""44""><a
                                                                                                href=""https://www.facebook.com/""
                                                                                                style=""color:#bd2225;text-decoration:underline""
                                                                                                target=""_blank""
                                                                                                ><img
                                                                                                    alt=""Biểu tượng Facebook""
                                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NZ8eWjjrcRIzSf97IShBwkN3hf6EAG7mwr6W_kVv5mlf6jXuaDyCZR-ZHxmIxbRCPnfGib4i13UY0rRnesmU-MdcGrTM2eq65bfR-TVMbW9BRZ42k4MYcppnxxUQcVyOuitL-E=s0-d-e1-ft#https://lolstatic-a.akamaihd.net/email-marketing/betabuddies/facebook-logo.png""
                                                                                                    style=""border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px""
                                                                                                    width=""44""
                                                                                                    height=""44""
                                                                                                    class=""CToWUd""
                                                                                                    data-bit=""iit""></a>
                                                                                        </td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""24"" height=""44""></td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""44"" height=""44""><a
                                                                                                href=""https://www.instagram.com/""
                                                                                                style=""color:#bd2225;text-decoration:underline""
                                                                                                target=""_blank""
                                                                                               ><img
                                                                                                    alt=""Biểu tượng Instagram""
                                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NZUedGKkwdQ9Jw0Y6ClifA4PDpAMyAW1-N0oAWzeWOkcqJmIjw5BHdJOBiVWHCOjj3duW-y3unrjqfIcT4-q92i1dDv5ljZKhjocQMimNWs1PnpumPVQ64k3JBtOtYDCrYTJFUV=s0-d-e1-ft#https://lolstatic-a.akamaihd.net/email-marketing/betabuddies/instagram-logo.png""
                                                                                                    style=""border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px""
                                                                                                    width=""44""
                                                                                                    height=""44""
                                                                                                    class=""CToWUd""
                                                                                                    data-bit=""iit""></a>
                                                                                        </td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""24"" height=""44""></td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""44"" height=""44""><a
                                                                                                href=""https://www.youtube.com/""
                                                                                                style=""color:#bd2225;text-decoration:underline""
                                                                                                target=""_blank""
                                                                                                ><img
                                                                                                    alt=""Biểu tượng YouTube""
                                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_Nbw5BguhKUzXGTPLZsJY9xNhnoGbwqSlFVubmXT-KvYiKA_WihAcokPFB5Ea-02DzZ_OjV7HO2EHFEA2itA_070a13moZT1eOK5cYTzdDH_qKykKVqjbfSSYG95ToiTmZ7qNw=s0-d-e1-ft#https://lolstatic-a.akamaihd.net/email-marketing/betabuddies/youtube-logo.png""
                                                                                                    style=""border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px""
                                                                                                    width=""44""
                                                                                                    height=""44""
                                                                                                    class=""CToWUd""
                                                                                                    data-bit=""iit""></a>
                                                                                        </td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""24"" height=""44""></td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""44"" height=""44""><a
                                                                                                href=""https://x.com/""
                                                                                                style=""color:#bd2225;text-decoration:underline""
                                                                                                target=""_blank""
                                                                                                ><img
                                                                                                    alt=""Biểu tượng Twitter""
                                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NYDPpMKFfKpK07U8PBz_ZZkCa3lxfy-wSHkgBALHkWzEbaBXgiPGCHsLabi4OzA0cewt01ygRh-io4GT0MpbRvRm41I5P4K8O3m5S_RIKGMuPVvPsxsHKqoeXY-cyl8K3yfLQ=s0-d-e1-ft#https://lolstatic-a.akamaihd.net/email-marketing/betabuddies/twitter-logo.png""
                                                                                                    style=""border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px""
                                                                                                    width=""44""
                                                                                                    height=""44""
                                                                                                    class=""CToWUd""
                                                                                                    data-bit=""iit""></a>
                                                                                        </td>
                                                                                        <td
                                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td colspan=""1""
                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:32px;border-collapse:collapse;font-family:inherit""
                                                                            height=""32"">
                                                                            <table
                                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                                width=""100%""></table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <table cellpadding=""0"" cellspacing=""0""
                                                                style=""padding:0;border:none;border-spacing:0;width:100%;margin:0 auto;border-collapse:collapse""
                                                                width=""100%"">
                                                                <tbody>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                                            align=""center""><span
                                                                                style=""display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit""><a
                                                                                    
                                                                                    style=""text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle""
                                                                                    
                                                                                    data-saferedirecturl=""https://www.google.com/url?q=https://links.riotgames.com/ls/click?upn%3DfrHghcUMWgUZ9OUHJJkDdaslin-2FqpxTNhpsWvP-2FdJrIzmy3Vb9wkwL7FAp39O-2FpyV3KcJTlQXFtw4C6etxvOOw-3D-3Dg9-8_CyPYuvKevH6nD6CMROzBiVQzIj9ecAtAT4i5qN8rzTPS70M0Tdj-2FnTRrzaK0fly80zNHaMqliceDQg7-2FI7Nzdxh6rc4O-2FlJGR-2FXhvyLnYZ7gf9Aw3ue5CqgeIu2Y-2B0ut36SxWVjDVHZhHRcb-2BLf4oLViFWt6BP6OfO6Ss0X2cbAaBqdSpqWxghXv-2FEwS0BFTsaaUSRCvl0WX5d7QjEla3SHyQxQ1OnXg8O-2FOzcOVJo-2B7Z1395NjjJC-2FslOKr5octrndcyDoXgrabyTDWtoaWP-2BRmw1hJpxjB98geqN3E1kBvEXfZzuoWjkizDCrtSNVPMx0SWGHUejW-2B-2BoRJaob6njY3ne-2FVtHMV5UxEBk2QiER9AjzN87Cf1tetplaxwk1nhpuWA7XvZR-2F1lG1VKHSjEWPP-2BDAMUD4t2Sktq9N8SqCzoZbihLOn9KpX8cTriDAhVi6731R3-2BcmU4JogPjoXPIfUssvqmmNOYKh3ntXwVnsz1Iil3CDaIkzy99qvhWEbeHTvL4ri0v6kmTw46cX8J1e0vu0hLzrHbEPoq9FWNhdkMpJC4gTXQY8nA-2FOhHeaIeRZcobynQVisoBbDbVdamDQIrOrWEHhN8HGUWiMAKuDzSQZeBba-2F3LYI7lTvxqEFVUM1s3i3fYMpbzRittkCDMghizSCTYq4EPeBpWQxBiYJbIyqTqf8jqwRdHzbE7qyH9T6n-2FJcmAKOgQjXkilY1c7NUoI5zXI6DKGNye0yO-2F02ROhHSnZt956VdejVtb8svxQnrv7tj25WYfoEpBvg1SqHBjbT91VwRQSMexbsOvVSlrMLCxZsI91FpLUYYXlgMJjbvJNphRf0-2FOf0WlaS3C1yHYlKoNGmUJitVEr5OMW-2FQl4gX2u93GTWMQ5cDhwoBD9r-2BtSUdACezUycbLI3ZpU3PVDGbZ0uiUpWu7WKteuEMrM7UVTcC7DFtUpDGE9S4or4dEPo0aMjgUstjEjYl1mR4xKBQSsizz-2F8W3XFv3w-3D&amp;source=gmail&amp;ust=1745531320354000&amp;usg=AOvVaw2XtrVtHXJwzY970jLPVO2z"">Chính
                                                                                    sách Quyền riêng tư</a></span><span
                                                                                style=""display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit""><img
                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NbM3Nyr77fPOiupqmiHDcc6ktxhIgsg0vznSDjfXk9IIZX3_DDLzI3WZHRBdbBY_YnLXAQm_LpTNGW_UJsOMfHLlHTvEtduHIu0A09Hna5b584BoHzW420iY5MMnKbtwAm1U37j1DmJnTT_66ldmILXpc1CII44z0RTyUrdEbb0QOBtZg=s0-d-e1-ft#http://cdn.mcauto-images-production.sendgrid.net/6c20475da3226ec8/e457af8c-5531-4df1-a265-127217b6d80a/8x8.png""
                                                                                    style=""border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px""
                                                                                    width=""4"" class=""CToWUd""
                                                                                    data-bit=""iit""><a
                                                                                   
                                                                                    style=""text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle""
                                                                                    
                                                                                    >Hỗ
                                                                                    trợ</a></span><span
                                                                                style=""display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit""><img
                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NbM3Nyr77fPOiupqmiHDcc6ktxhIgsg0vznSDjfXk9IIZX3_DDLzI3WZHRBdbBY_YnLXAQm_LpTNGW_UJsOMfHLlHTvEtduHIu0A09Hna5b584BoHzW420iY5MMnKbtwAm1U37j1DmJnTT_66ldmILXpc1CII44z0RTyUrdEbb0QOBtZg=s0-d-e1-ft#http://cdn.mcauto-images-production.sendgrid.net/6c20475da3226ec8/e457af8c-5531-4df1-a265-127217b6d80a/8x8.png""
                                                                                    style=""border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px""
                                                                                    width=""4"" class=""CToWUd""
                                                                                    data-bit=""iit""><a
                                                                                   
                                                                                    style=""text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle""
                                                                                 >Điều
                                                                                    khoản Sử dụng</a></span></td>
                                                                    </tr>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td colspan=""1""
                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit""
                                                                            height=""16"">
                                                                            <table
                                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                                width=""100%""></table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center""><span style=""font-family:inherit"">Đây là dịch
                                                                vụ thư thông báo.</span></td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td colspan=""1""
                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit""
                                                            height=""16"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%""></table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center""><span style=""font-family:inherit"">CineX Cinema, Trinh Van Bo Street, Nam Tu Liem, Ha Noi</span></td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td colspan=""1""
                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit""
                                                            height=""16"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%""></table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center""><span style=""font-family:inherit"">© năm 2025
                                                                - bởi CineX Cinema.</span>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                            width=""72"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit""
                                            height=""64"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>


    </div>
</body>

</html>
Đang hiển thị 9041612706522265649.";

                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(email.Trim());
                    mail.From = new MailAddress("thaothaobatbai123@gmail.com", "Hệ thống hỗ trợ");
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.Body = body;
                    mail.BodyEncoding = Encoding.UTF8;

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
        public async Task<bool> SendOtpEmail(string email, string optCode)
        {
            try
            {

                string subject = "Your OTP Code";
                string body = $@"
       
<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Document</title>
</head>

<body>
    <div style=""padding:0;margin:0;height:100%;width:100%;font-family:Arial,'Times New Roman','Calibri'"">


        <div style=""margin:0 auto;max-width:600px;display:block;background:#f0f0f0;font-family:inherit"">
            <table cellpadding=""0"" cellspacing=""0""
                style=""padding:0;border-spacing:0;background:#f0f0f0;border:0;margin:0;text-align:center;vertical-align:middle;font-weight:500;table-layout:fixed;border-collapse:collapse;height:100%;width:100%;line-height:100%""
                width=""100%"" height=""100%"" align=""center"" valign=""middle"">
                <tbody>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td
                            style=""margin:0;padding:0;border:none;border-spacing:0;background:#f0f0f0;border-collapse:collapse;font-family:inherit"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;border-spacing:0;border:0;padding:0;width:100%;border-collapse:collapse""
                                width=""100%"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center""></td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td
                                            style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">

                                            <table cellpadding=""0"" cellspacing=""0""
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%"">
                                                <tbody>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;background-image:url(https://res.cloudinary.com/dw44ghjmu/image/upload/v1745446945/LogoEmailCinex_iuwqqe.png);background-size:cover;width:612px;height:146px;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            width=""612"" height=""146""
                                                            background=""https://res.cloudinary.com/dw44ghjmu/image/upload/v1745446945/LogoEmailCinex_iuwqqe.png""
                                                            align=""center""></td>
                                                    </tr>
                                                </tbody>
                                            </table>


                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""1""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:24px;border-collapse:collapse;font-family:inherit""
                                            height=""24"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">
                                            <table cellpadding=""0"" cellspacing=""0""
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%"">
                                                <tbody>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                                            width=""72"" height=""100%"">
                                                            <div
                                                                style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                                            </div>
                                                        </td>
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <h1
                                                                style=""font-size:32px;font-weight:500;letter-spacing:0.01em;color:#141212;text-align:center;line-height:39px;margin:0;font-family:inherit"">
                                                                Mã Xác Minh</h1>
                                                        </td>
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                                            width=""72"" height=""100%"">
                                                            <div
                                                                style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td colspan=""1""
                            style=""margin:0;padding:0;border:none;border-spacing:0;height:0px;border-collapse:collapse;font-family:inherit""
                            height=""0"">
                            <table
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                width=""100%"">
                            </table>
                        </td>
                    </tr>
                    
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center"">
                            </td>
                    </tr>
                    
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                width=""100%"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit""
                                            height=""64"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""><p>Để xác minh tài khoản của bạn, hãy nhập mã này vào CineX:</p></table>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                            width=""72"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                            </div>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">
                                            <table cellpadding=""0"" cellspacing=""0""
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;background-color:#f9f9f9;border-collapse:collapse""
                                                width=""100%"" bgcolor=""#F9F9F9"">
                                                <tbody>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td colspan=""3""
                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:40px;border-collapse:collapse;font-family:inherit""
                                                            height=""40"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%""></table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit""
                                                            width=""38"" height=""100%"">
                                                            <div
                                                                style=""height:100%;overflow:hidden;width:38px;font-family:inherit"">
                                                            </div>
                                                        </td>
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse""
                                                                width=""100%"">
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <h1
                                                                                style=""font-size:40px;font-weight:700;line-height:100%;color:#c51e1e;margin:0;text-align:center;font-family:inherit"">
                                                                                {optCode}
                                                                            </h1>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                    
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:38px;border-collapse:collapse;font-family:inherit""
                                                            width=""38"" height=""100%"">
                                                            <div
                                                                style=""height:100%;overflow:hidden;width:38px;font-family:inherit"">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td colspan=""3""
                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit""
                                                            height=""48"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%""></table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                            width=""72"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit""
                                            height=""48"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center""></td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:16px;text-align:center;line-height:140%;letter-spacing:-0.01em;color:#666;border-collapse:collapse""
                                width=""100%"" align=""center"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit""
                                            width=""100"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:100px;font-family:inherit"">
                                            </div>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">Mã xác minh sẽ hết hạn sau 48 giờ.</td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit""
                                            width=""100"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:100px;font-family:inherit"">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:24px;border-collapse:collapse;font-family:inherit""
                                            height=""24"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center""></td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:16px;text-align:center;line-height:140%;letter-spacing:-0.01em;color:#666;border-collapse:collapse""
                                width=""100%"" align=""center"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit""
                                            width=""100"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:100px;font-family:inherit"">
                                            </div>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">Nếu bạn không yêu cầu mã, bạn có thể bỏ qua tin nhắn.</td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:100px;border-collapse:collapse;font-family:inherit""
                                            width=""100"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:100px;font-family:inherit"">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:20px;border-collapse:collapse;font-family:inherit""
                                            height=""80"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td colspan=""1""
                            style=""margin:0;padding:0;border:none;border-spacing:0;height:0px;border-collapse:collapse;font-family:inherit""
                            height=""0"">
                            <table
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                width=""100%""></table>
                        </td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center""></td>
                    </tr>
                    <tr
                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                            align=""center"">
                            <table cellpadding=""0"" cellspacing=""0""
                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                width=""100%"">
                                <tbody>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                            width=""72"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                            </div>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                            align=""center"">
                                            <table cellpadding=""0"" cellspacing=""0""
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;font-size:11.24px;line-height:140%;letter-spacing:-0.01em;color:#999;table-layout:fixed;border-collapse:collapse""
                                                width=""100%"">
                                                <tbody>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;display:inline-table;width:auto;border-collapse:collapse"">
                                                                <tbody>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        
                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;width:32px;border-collapse:collapse;font-family:inherit""
                                                                            width=""32"" align=""center""></td>
                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                                            align=""center""><img
                                                                                src=""https://res.cloudinary.com/dw44ghjmu/image/upload/v1745447860/e9acac58-056e-4b69-bc95-0576c455b5f4_rtybyk.png""
                                                                                style=""border:0;height:auto;line-height:100%;outline:none;text-decoration:none;display:inline-block;width:187px""
                                                                                width=""187"" class=""CToWUd""
                                                                                data-bit=""iit""></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <table cellpadding=""0"" cellspacing=""0""
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%"">
                                                                <tbody>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td colspan=""1""
                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:48px;border-collapse:collapse;font-family:inherit""
                                                                            height=""48"">
                                                                            <table
                                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                                width=""100%""></table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                                            align=""center"">
                                                                            <table cellpadding=""0"" cellspacing=""0""
                                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;table-layout:fixed;border-collapse:collapse""
                                                                                width=""100%"">
                                                                                <tbody>
                                                                                    <tr
                                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;height:44px;width:100%;border-collapse:collapse;font-family:inherit"">
                                                                                        <td
                                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                                        </td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""44"" height=""44""><a
                                                                                                href=""https://www.facebook.com/""
                                                                                                style=""color:#bd2225;text-decoration:underline""
                                                                                                target=""_blank""
                                                                                                ><img
                                                                                                    alt=""Biểu tượng Facebook""
                                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NZ8eWjjrcRIzSf97IShBwkN3hf6EAG7mwr6W_kVv5mlf6jXuaDyCZR-ZHxmIxbRCPnfGib4i13UY0rRnesmU-MdcGrTM2eq65bfR-TVMbW9BRZ42k4MYcppnxxUQcVyOuitL-E=s0-d-e1-ft#https://lolstatic-a.akamaihd.net/email-marketing/betabuddies/facebook-logo.png""
                                                                                                    style=""border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px""
                                                                                                    width=""44""
                                                                                                    height=""44""
                                                                                                    class=""CToWUd""
                                                                                                    data-bit=""iit""></a>
                                                                                        </td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""24"" height=""44""></td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""44"" height=""44""><a
                                                                                                href=""https://www.instagram.com/""
                                                                                                style=""color:#bd2225;text-decoration:underline""
                                                                                                target=""_blank""
                                                                                               ><img
                                                                                                    alt=""Biểu tượng Instagram""
                                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NZUedGKkwdQ9Jw0Y6ClifA4PDpAMyAW1-N0oAWzeWOkcqJmIjw5BHdJOBiVWHCOjj3duW-y3unrjqfIcT4-q92i1dDv5ljZKhjocQMimNWs1PnpumPVQ64k3JBtOtYDCrYTJFUV=s0-d-e1-ft#https://lolstatic-a.akamaihd.net/email-marketing/betabuddies/instagram-logo.png""
                                                                                                    style=""border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px""
                                                                                                    width=""44""
                                                                                                    height=""44""
                                                                                                    class=""CToWUd""
                                                                                                    data-bit=""iit""></a>
                                                                                        </td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""24"" height=""44""></td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""44"" height=""44""><a
                                                                                                href=""https://www.youtube.com/""
                                                                                                style=""color:#bd2225;text-decoration:underline""
                                                                                                target=""_blank""
                                                                                                ><img
                                                                                                    alt=""Biểu tượng YouTube""
                                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_Nbw5BguhKUzXGTPLZsJY9xNhnoGbwqSlFVubmXT-KvYiKA_WihAcokPFB5Ea-02DzZ_OjV7HO2EHFEA2itA_070a13moZT1eOK5cYTzdDH_qKykKVqjbfSSYG95ToiTmZ7qNw=s0-d-e1-ft#https://lolstatic-a.akamaihd.net/email-marketing/betabuddies/youtube-logo.png""
                                                                                                    style=""border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px""
                                                                                                    width=""44""
                                                                                                    height=""44""
                                                                                                    class=""CToWUd""
                                                                                                    data-bit=""iit""></a>
                                                                                        </td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:24px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""24"" height=""44""></td>
                                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;width:44px;height:44px;border-collapse:collapse;font-family:inherit""
                                                                                            width=""44"" height=""44""><a
                                                                                                href=""https://x.com/""
                                                                                                style=""color:#bd2225;text-decoration:underline""
                                                                                                target=""_blank""
                                                                                                ><img
                                                                                                    alt=""Biểu tượng Twitter""
                                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NYDPpMKFfKpK07U8PBz_ZZkCa3lxfy-wSHkgBALHkWzEbaBXgiPGCHsLabi4OzA0cewt01ygRh-io4GT0MpbRvRm41I5P4K8O3m5S_RIKGMuPVvPsxsHKqoeXY-cyl8K3yfLQ=s0-d-e1-ft#https://lolstatic-a.akamaihd.net/email-marketing/betabuddies/twitter-logo.png""
                                                                                                    style=""border:0;line-height:100%;outline:none;text-decoration:none;width:44px;height:44px""
                                                                                                    width=""44""
                                                                                                    height=""44""
                                                                                                    class=""CToWUd""
                                                                                                    data-bit=""iit""></a>
                                                                                        </td>
                                                                                        <td
                                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td colspan=""1""
                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:32px;border-collapse:collapse;font-family:inherit""
                                                                            height=""32"">
                                                                            <table
                                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                                width=""100%""></table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center"">
                                                            <table cellpadding=""0"" cellspacing=""0""
                                                                style=""padding:0;border:none;border-spacing:0;width:100%;margin:0 auto;border-collapse:collapse""
                                                                width=""100%"">
                                                                <tbody>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                                            align=""center""><span
                                                                                style=""display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit""><a
                                                                                    
                                                                                    style=""text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle""
                                                                                    
                                                                                    data-saferedirecturl=""https://www.google.com/url?q=https://links.riotgames.com/ls/click?upn%3DfrHghcUMWgUZ9OUHJJkDdaslin-2FqpxTNhpsWvP-2FdJrIzmy3Vb9wkwL7FAp39O-2FpyV3KcJTlQXFtw4C6etxvOOw-3D-3Dg9-8_CyPYuvKevH6nD6CMROzBiVQzIj9ecAtAT4i5qN8rzTPS70M0Tdj-2FnTRrzaK0fly80zNHaMqliceDQg7-2FI7Nzdxh6rc4O-2FlJGR-2FXhvyLnYZ7gf9Aw3ue5CqgeIu2Y-2B0ut36SxWVjDVHZhHRcb-2BLf4oLViFWt6BP6OfO6Ss0X2cbAaBqdSpqWxghXv-2FEwS0BFTsaaUSRCvl0WX5d7QjEla3SHyQxQ1OnXg8O-2FOzcOVJo-2B7Z1395NjjJC-2FslOKr5octrndcyDoXgrabyTDWtoaWP-2BRmw1hJpxjB98geqN3E1kBvEXfZzuoWjkizDCrtSNVPMx0SWGHUejW-2B-2BoRJaob6njY3ne-2FVtHMV5UxEBk2QiER9AjzN87Cf1tetplaxwk1nhpuWA7XvZR-2F1lG1VKHSjEWPP-2BDAMUD4t2Sktq9N8SqCzoZbihLOn9KpX8cTriDAhVi6731R3-2BcmU4JogPjoXPIfUssvqmmNOYKh3ntXwVnsz1Iil3CDaIkzy99qvhWEbeHTvL4ri0v6kmTw46cX8J1e0vu0hLzrHbEPoq9FWNhdkMpJC4gTXQY8nA-2FOhHeaIeRZcobynQVisoBbDbVdamDQIrOrWEHhN8HGUWiMAKuDzSQZeBba-2F3LYI7lTvxqEFVUM1s3i3fYMpbzRittkCDMghizSCTYq4EPeBpWQxBiYJbIyqTqf8jqwRdHzbE7qyH9T6n-2FJcmAKOgQjXkilY1c7NUoI5zXI6DKGNye0yO-2F02ROhHSnZt956VdejVtb8svxQnrv7tj25WYfoEpBvg1SqHBjbT91VwRQSMexbsOvVSlrMLCxZsI91FpLUYYXlgMJjbvJNphRf0-2FOf0WlaS3C1yHYlKoNGmUJitVEr5OMW-2FQl4gX2u93GTWMQ5cDhwoBD9r-2BtSUdACezUycbLI3ZpU3PVDGbZ0uiUpWu7WKteuEMrM7UVTcC7DFtUpDGE9S4or4dEPo0aMjgUstjEjYl1mR4xKBQSsizz-2F8W3XFv3w-3D&amp;source=gmail&amp;ust=1745531320354000&amp;usg=AOvVaw2XtrVtHXJwzY970jLPVO2z"">Chính
                                                                                    sách Quyền riêng tư</a></span><span
                                                                                style=""display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit""><img
                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NbM3Nyr77fPOiupqmiHDcc6ktxhIgsg0vznSDjfXk9IIZX3_DDLzI3WZHRBdbBY_YnLXAQm_LpTNGW_UJsOMfHLlHTvEtduHIu0A09Hna5b584BoHzW420iY5MMnKbtwAm1U37j1DmJnTT_66ldmILXpc1CII44z0RTyUrdEbb0QOBtZg=s0-d-e1-ft#http://cdn.mcauto-images-production.sendgrid.net/6c20475da3226ec8/e457af8c-5531-4df1-a265-127217b6d80a/8x8.png""
                                                                                    style=""border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px""
                                                                                    width=""4"" class=""CToWUd""
                                                                                    data-bit=""iit""><a
                                                                                   
                                                                                    style=""text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle""
                                                                                    
                                                                                    >Hỗ
                                                                                    trợ</a></span><span
                                                                                style=""display:inline;vertical-align:middle;font-weight:800;font-size:12.64px;letter-spacing:0.08em;white-space:nowrap;font-family:inherit""><img
                                                                                    src=""https://ci3.googleusercontent.com/meips/ADKq_NbM3Nyr77fPOiupqmiHDcc6ktxhIgsg0vznSDjfXk9IIZX3_DDLzI3WZHRBdbBY_YnLXAQm_LpTNGW_UJsOMfHLlHTvEtduHIu0A09Hna5b584BoHzW420iY5MMnKbtwAm1U37j1DmJnTT_66ldmILXpc1CII44z0RTyUrdEbb0QOBtZg=s0-d-e1-ft#http://cdn.mcauto-images-production.sendgrid.net/6c20475da3226ec8/e457af8c-5531-4df1-a265-127217b6d80a/8x8.png""
                                                                                    style=""border:0;height:auto;line-height:100%;outline:none;text-decoration:none;width:4px;vertical-align:middle;margin:4px 16px""
                                                                                    width=""4"" class=""CToWUd""
                                                                                    data-bit=""iit""><a
                                                                                   
                                                                                    style=""text-decoration:none;text-transform:uppercase;color:#999;vertical-align:middle""
                                                                                 >Điều
                                                                                    khoản Sử dụng</a></span></td>
                                                                    </tr>
                                                                    <tr
                                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                                        <td colspan=""1""
                                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit""
                                                                            height=""16"">
                                                                            <table
                                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                                width=""100%""></table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center""><span style=""font-family:inherit"">Đây là dịch
                                                                vụ thư thông báo.</span></td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td colspan=""1""
                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit""
                                                            height=""16"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%""></table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center""><span style=""font-family:inherit"">CineX Cinema, Trinh Van Bo Street, Nam Tu Liem, Ha Noi</span></td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td colspan=""1""
                                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:16px;border-collapse:collapse;font-family:inherit""
                                                            height=""16"">
                                                            <table
                                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                                width=""100%""></table>
                                                        </td>
                                                    </tr>
                                                    <tr
                                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;text-align:center;border-collapse:collapse;font-family:inherit""
                                                            align=""center""><span style=""font-family:inherit"">© năm 2025
                                                                - bởi CineX Cinema.</span>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td style=""margin:0;padding:0;border:none;border-spacing:0;height:100%;overflow:hidden;width:72px;border-collapse:collapse;font-family:inherit""
                                            width=""72"" height=""100%"">
                                            <div style=""height:100%;overflow:hidden;width:72px;font-family:inherit"">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr
                                        style=""margin:0;padding:0;border:none;border-spacing:0;border-collapse:collapse;font-family:inherit"">
                                        <td colspan=""3""
                                            style=""margin:0;padding:0;border:none;border-spacing:0;height:64px;border-collapse:collapse;font-family:inherit""
                                            height=""64"">
                                            <table
                                                style=""margin:0;padding:0;border:none;border-spacing:0;width:100%;border-collapse:collapse""
                                                width=""100%""></table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>


    </div>
</body>

</html>
Đang hiển thị 3243575109743965752.";

                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(email.Trim());
                    mail.From = new MailAddress("thaothaobatbai123@gmail.com", "Hệ thống hỗ trợ");
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.Body = body;
                    mail.BodyEncoding = Encoding.UTF8;

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
                Console.WriteLine($"Lỗi gửi email OTP: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> SendMailRefundAll(GetInfoRefundRes req)
        {
            string email = req.Email; 
            string subject = "Thông Báo Hoàn Điểm Khi Gặp Sự Cố";
            string body = $@"<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Document</title>
    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css"">
</head>

<body>
    <div class="""">
        <div class=""aHl""></div>
        <div id="":n4"" tabindex=""-1""></div>
        <div id="":ne"" class=""ii gt""
            jslog=""20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTgyOTAxMzAyNzQ1OTIxOTMwNCJd; 4:WyIjbXNnLWY6MTgyOTAxMzAyNzQ1OTIxOTMwNCIsbnVsbCxudWxsLG51bGwsMSwwLFsxLDAsMF0sMTE0LDgyNCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsMSxudWxsLG51bGwsWzNdLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLDBd"">
            <div id="":nf"" class=""a3s aiL msg8701763580526252854""><u></u>
                <div lang=""en"" link=""#DD0000"" vlink=""#DD0000"" class=""m_8701763580526252854emailify""
                    style=""word-spacing:normal;background-color:#f5f5f5"">
                    <div style=""background-color:#f5f5f5"" lang=""en"" dir=""auto"">
                        <div class=""m_-1228065499387620760h m_-1228065499387620760e m_-1228065499387620760eya m_-1228065499387620760mg m_-1228065499387620760ys"" style=""margin:0 auto;max-width:600px"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""width:100%"">
                                <tbody>
                                    <tr style=""vertical-align:top"">
                                        <td background=""https://ci3.googleusercontent.com/meips/ADKq_NY1IiJ5HLSPwQ8Sbx8PxqoJH9OI2QPBvLp3vZu5_28fzYGnpKjhVFPfaqOaD-NcS1f--Z9HJO7tYY2_sFS17CaYk7BX_7MOiWFg4tCs3stR7qZ1=s0-d-e1-ft#https://assets.monica.im/assets/img/email-subscriptions-1.png"" style=""background:transparent url('https://ci3.googleusercontent.com/meips/ADKq_NY1IiJ5HLSPwQ8Sbx8PxqoJH9OI2QPBvLp3vZu5_28fzYGnpKjhVFPfaqOaD-NcS1f--Z9HJO7tYY2_sFS17CaYk7BX_7MOiWFg4tCs3stR7qZ1=s0-d-e1-ft#https://assets.monica.im/assets/img/email-subscriptions-1.png') no-repeat center center/cover;background-size:cover;background-position:center center;background-repeat:no-repeat;padding:32px 32px 32px 32px;vertical-align:top;height:112px"" height=""112"">
                                            <div style=""margin:0px auto"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""width:100%;margin:0"">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""width:100%;margin:0"">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td align=""center"" class=""m_-1228065499387620760m"" style=""font-size:0;padding:0;padding-bottom:8px;word-break:break-word"">
                                                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""border-collapse:collapse;border-spacing:0"">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style=""width:100px"">
                                                                                                <img alt=""CineX Logo"" src=""https://res.cloudinary.com/dw44ghjmu/image/upload/v1745400720/cinexLogo_w6wqyi.png"" style=""border:0;display:block;outline:none;text-decoration:none;height:auto;width:100%;font-size:13px"" width=""100"" height=""auto"" class=""CToWUd"" data-bit=""iit"">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class=""m_-1228065499387620760m"" style=""font-size:0;padding:0;padding-bottom:8px;word-break:break-word"" aria-hidden=""true"">
                                                                                <div style=""height:6px;line-height:6px"">&hairsp;</div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align=""center"" class=""m_-1228065499387620760x m_-1228065499387620760m"" style=""font-size:0;padding-bottom:8px;word-break:break-word"">
                                                                                <div style=""text-align:center"">
                                                                                    <p style=""Margin:0;text-align:center"">
                                                                                        <span style=""font-size:28px;font-family:'Inter','Arial',sans-serif;font-weight:700;color:#262626;line-height:121%"">Thông Báo Hoàn Điểm Khi Gặp Sự Cố!</span>
                                                                                    </p>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align=""center"" class=""m_-1228065499387620760x"" style=""font-size:0;padding-bottom:0;word-break:break-word"">
                                                                                <div style=""text-align:center"">
                                                                                    <p style=""Margin:0;text-align:center"">
                                                                                        <span style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:400;color:#595959;line-height:150%"">CineX sẽ hoàn trả  {req.PointRefund} điểm thưởng (tương đương  {req.PointRefund} VNĐ) khi rạp gặp sự cố kỹ thuật ảnh hưởng đến trải nghiệm xem phim của Quý khách.Số điểm này quý khách có thể mua lại vé ở những lần tiếp theo.</span>
                                                                                    </p>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        
                        
                        <div class=""m_8701763580526252854h m_8701763580526252854e m_8701763580526252854eya m_8701763580526252854mg m_8701763580526252854ys""
                            style=""margin:0 auto;max-width:600px"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""width:100%"">
                                <tbody>
                                    <tr style=""vertical-align:top"">
                                        <td background=""https://res.cloudinary.com/dw44ghjmu/image/upload/v1745400366/NenEmail_vw9rja.png"" style=""background:transparent url('https://res.cloudinary.com/dw44ghjmu/image/upload/v1745400366/NenEmail_vw9rja.png') no-repeat center center/cover;
                                        background-size:cover;
                                        background-position:center center;
                                        background-repeat:no-repeat;
                                        padding:32px 164px 32px 164px;
                                        vertical-align:middle;
                                        height:236px"" height=""236"">
                                            <div style=""margin:0px auto"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
                                                    style=""width:100%;margin:0"">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <table border=""0"" cellpadding=""0"" cellspacing=""0""
                                                                    role=""presentation"" style=""width:100%;margin:0"">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td class=""m_8701763580526252854m""
                                                                                style=""font-size:0;padding:0;padding-bottom:4px;word-break:break-word""
                                                                                aria-hidden=""true"">
                                                                                <div
                                                                                    style=""height:109px;line-height:109px"">
                                                                                    &hairsp;</div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align=""left""
                                                                                class=""m_8701763580526252854x m_8701763580526252854d m_8701763580526252854xr m_8701763580526252854m""
                                                                                style=""font-size:0;padding-bottom:4px;word-break:break-word"">
                                                                                <div style=""text-align:left"">
                                                                                    <p style=""Margin:0;text-align:left"">
                                                                                        <span
                                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:600;color:#262626;line-height:150%"">CineX Vip Membership</span>
                                                                                            </span>
                                                                                    </p>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align=""left""
                                                                                class=""m_8701763580526252854x m_8701763580526252854d m_8701763580526252854xr m_8701763580526252854m""
                                                                                style=""font-size:0;padding-bottom:4px;word-break:break-word"">
                                                                                <div style=""text-align:left"">
                                                                                    <p style=""Margin:0;text-align:left"">
                                                                                        <span
                                                                                            style=""font-size:12px;font-family:'Inter','Arial',sans-serif;font-weight:400;color:#595959;line-height:150%"">Nâng cấp thẻ ngay để nhận thêm nhiều ưu đãi</span>
                                                                                    </p>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                        <tr>
                                                                            <td class=""m_8701763580526252854m""
                                                                                style=""font-size:0;padding:0;padding-bottom:4px;word-break:break-word""
                                                                                aria-hidden=""true"">
                                                                                <div
                                                                                    style=""height:24px;line-height:24px"">
                                                                                    &hairsp;</div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align=""center""
                                                                                class=""m_8701763580526252854b""
                                                                                style=""font-size:0;padding:0;padding-bottom:0;word-break:break-word"">
                                                                                <table border=""0"" cellpadding=""0""
                                                                                    cellspacing=""0"" role=""presentation""
                                                                                    style=""border-collapse:separate;min-width:91px;line-height:100%"">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td align=""center""
                                                                                                bgcolor=""#6841ea""
                                                                                                role=""presentation""
                                                                                                style=""background:#6841ea;border:none;border-radius:8px 8px 8px 8px;vertical-align:middle""
                                                                                                valign=""middle""> <a
                                                                                                    href=""http://localhost:4200/""
                                                                                                    style=""display:inline-block;min-width:91px;background-color:#6841ea;color:#ffffff;font-family:'Inter','Arial',sans-serif;font-size:13px;font-weight:normal;line-height:100%;margin:0;text-decoration:none;text-transform:none;padding:14px 20px 14px 20px;border-radius:8px 8px 8px 8px""
                                                                                                    target=""_blank"">
                                                                                                    <span
                                                                                                        style=""font-size:14px;font-family:'Inter','Arial',sans-serif;font-weight:700;color:#ffffff;line-height:121%"">
                                                                                                        Nâng Cấp </span></a>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class=""m_8701763580526252854r m_8701763580526252854eya m_8701763580526252854ys""
                            style=""background:#fffffe;background-color:#fffffe;margin:0px auto;max-width:600px"">
                            <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
                                style=""background:#fffffe;background-color:#fffffe;width:100%"">
                                <tbody>
                                    <tr>
                                        <td
                                            style=""border:none;direction:ltr;font-size:0;padding:32px 32px 16px 32px;text-align:left"">
                                            <div class=""m_8701763580526252854h1""
                                                style=""font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:middle;width:100%"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
                                                    style=""border:none;vertical-align:middle"" width=""100%"">
                                                    <tbody>
                                                        <tr>
                                                            <td align=""center"" class=""m_8701763580526252854x""
                                                                style=""font-size:0;word-break:break-word"">
                                                                <div style=""text-align:center"">
                                                                    <p style=""Margin:0;text-align:center""><span
                                                                            style=""font-size:28px;font-family:'Inter','Arial',sans-serif;font-weight:700;color:#262626;line-height:121%"">Hướng
                                                                            Dẫn Đăng Ký Tài Khoản CineX</span></p>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class=""m_8701763580526252854r m_8701763580526252854eya m_8701763580526252854ys""
                            style=""background:#fffffe;background-color:#fffffe;margin:0px auto;max-width:600px"">
                            <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
                                style=""background:#fffffe;background-color:#fffffe;width:100%"">
                                <tbody>
                                    <tr>
                                        <td
                                            style=""border:none;direction:ltr;font-size:0;padding:16px 32px 16px 32px;text-align:left"">


                                            <div class=""m_8701763580526252854c""
                                                style=""font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:middle;width:100%"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
                                                    style=""border:none;vertical-align:middle"" width=""100%"">
                                                    <tbody>
                                                        <tr>
                                                            <td align=""left""
                                                                class=""m_8701763580526252854x m_8701763580526252854m""
                                                                style=""font-size:0;padding-bottom:8px;word-break:break-word"">
                                                                <div style=""text-align:left"">
                                                                    <p style=""Margin:0;text-align:left""><span
                                                                            style=""font-size:20px;font-family:'Inter','Arial',sans-serif;font-weight:600;color:#000000;line-height:120%"">Các
                                                                            bước đăng ký và nhận điểm thành viên</span>
                                                                    </p>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align=""left"" class=""m_8701763580526252854x""
                                                                style=""font-size:0;padding-bottom:12px;word-break:break-word"">
                                                                <div style=""text-align:left"">
                                                                    <p style=""Margin:0;text-align:left""><span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:600;color:#000000;line-height:150%"">Bước
                                                                            1: Truy cập vào trang CineX.com</span></p>
                                                                    <p
                                                                        style=""Margin:0;text-align:left;padding-left:10px"">
                                                                        <span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:400;color:#595959;line-height:150%"">Trang
                                                                            web dành riêng cho máy tính và tương thích
                                                                            với các thiết bị.</span>
                                                                    </p>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align=""left"" class=""m_8701763580526252854x""
                                                                style=""font-size:0;padding-bottom:12px;word-break:break-word"">
                                                                <div style=""text-align:left"">
                                                                    <p style=""Margin:0;text-align:left""><span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:600;color:#000000;line-height:150%"">Bước
                                                                            2: Tạo tài khoản mới</span></p>
                                                                    <p
                                                                        style=""Margin:0;text-align:left;padding-left:10px"">
                                                                        <span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:400;color:#595959;line-height:150%"">Mở
                                                                            trang web và nhấn vào ""Đăng ký"". Điền đầy đủ
                                                                            thông tin cá nhân bao gồm: họ tên, email, số
                                                                            điện thoại và mật khẩu. Đảm bảo sử dụng
                                                                            email và số điện thoại chính xác để nhận
                                                                            thông báo.</span>
                                                                    </p>
                                                                    <p
                                                                        style=""Margin:0;text-align:left;padding-left:10px"">
                                                                        <span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:500;color:#ff5722;line-height:150%"">Lưu
                                                                            ý: Nếu bạn đã từng sử dụng dịch vụ của chúng
                                                                            tôi mà chưa đăng nhập, bạn có thể sử dụng
                                                                            email đã đăng ký nhận thông tin trước đó để
                                                                            làm tài khoản, hệ thống sẽ có sẵn những dữ
                                                                            liệu đó trong email của bạn.</span>
                                                                    </p>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align=""left"" class=""m_8701763580526252854x""
                                                                style=""font-size:0;padding-bottom:12px;word-break:break-word"">
                                                                <div style=""text-align:left"">
                                                                    <p style=""Margin:0;text-align:left""><span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:600;color:#000000;line-height:150%"">Bước
                                                                            3: Đăng nhập</span></p>
                                                                    <p
                                                                        style=""Margin:0;text-align:left;padding-left:10px"">
                                                                        <span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:400;color:#595959;line-height:150%"">Bạn
                                                                            sẽ đăng nhập bằng tài khoản mật khẩu vừa tạo
                                                                            ở trang chủ Cinex.com.</span>
                                                                    </p>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align=""left"" class=""m_8701763580526252854x""
                                                                style=""font-size:0;padding-bottom:12px;word-break:break-word"">
                                                                <div style=""text-align:left"">
                                                                    <p style=""Margin:0;text-align:left""><span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:600;color:#000000;line-height:150%"">Bước
                                                                            4: Xem thông tin vé</span></p>
                                                                    <p
                                                                        style=""Margin:0;text-align:left;padding-left:10px"">
                                                                        <span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:400;color:#595959;line-height:150%"">Sau
                                                                            khi đăng nhập thành công hãy vào Thông tin
                                                                            vé ở phần Profile, hệ thống sẽ hiển thị toàn
                                                                            bộ vé mà bạn đã đặt (Bạn có thể xem toàn bộ
                                                                            những vé đã đặt bằng tài khoản email ngay cả
                                                                            trước khi đăng kí).</span>
                                                                    </p>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align=""left"" class=""m_8701763580526252854x""
                                                                style=""font-size:0;padding-bottom:12px;word-break:break-word"">
                                                                <div style=""text-align:left"">
                                                                    <p style=""Margin:0;text-align:left""><span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:600;color:#000000;line-height:150%"">Bước
                                                                            5: Nhận điểm thành viên</span></p>
                                                                    <p
                                                                        style=""Margin:0;text-align:left;padding-left:10px"">
                                                                        <span
                                                                            style=""font-size:16px;font-family:'Inter','Arial',sans-serif;font-weight:400;color:#595959;line-height:150%"">Vào
                                                                            Reward để nhận điểm thành viên (Điểm sẽ được
                                                                            dùng cho mục đích chuyền đổi khi thanh toán:
                                                                            1000 điểm tương đương với 10.000
                                                                            VNĐ).</span>
                                                                    </p>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>



                        <div class=""m_8701763580526252854r m_8701763580526252854eya m_8701763580526252854ys""
                            style=""background:#fffffe;background-color:#fffffe;margin:0px auto;max-width:600px"">
                            <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
                                style=""background:#fffffe;background-color:#fffffe;width:100%"">
                                <tbody>
                                    <tr>
                                        <td
                                            style=""border:none;direction:ltr;font-size:0;padding:0px 32px 80px 32px;text-align:left"">
                                            <div class=""m_8701763580526252854h1""
                                                style=""font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:middle;width:100%"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
                                                    style=""border:none;vertical-align:middle"" width=""100%"">
                                                    <tbody>
                                                        <tr>
                                                            <td align=""center"" class=""m_8701763580526252854b""
                                                                style=""font-size:0;padding:0;word-break:break-word"">
                                                                <table border=""0"" cellpadding=""0"" cellspacing=""0""
                                                                    role=""presentation""
                                                                    style=""border-collapse:separate;min-width:91px;line-height:100%"">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td align=""center"" bgcolor=""#6841ea""
                                                                                role=""presentation""
                                                                                style=""background:#6841ea;border:none;border-radius:8px 8px 8px 8px;vertical-align:middle""
                                                                                valign=""middle""> <a
                                                                                    href=""http://localhost:4200/""
                                                                                    style=""display:inline-block;min-width:91px;background-color:#6841ea;color:#ffffff;font-family:'Inter','Arial',sans-serif;font-size:13px;font-weight:normal;line-height:100%;margin:0;text-decoration:none;text-transform:none;padding:14px 20px 14px 20px;border-radius:8px 8px 8px 8px""
                                                                                    target=""_blank"">
                                                                                    <span href=""http://localhost:4200/""
                                                                                        style=""font-size:14px;font-family:'Inter','Arial',sans-serif;font-weight:700;color:#ffffff;line-height:121%"">Đăng
                                                                                        ký</span></a></td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class=""m_8701763580526252854r m_8701763580526252854eya m_8701763580526252854ys""
                            style=""background:#000000;background-color:#000000;margin:0px auto;max-width:600px"">
                            <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
                                style=""background:#000000;background-color:#000000;width:100%;max-width:600px"">
                                <tbody>
                                    <tr>
                                        <td align=""center"">
                                            <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" border=""0""
                                                style=""max-width:600px;width:100%"">
                                                <tbody>
                                                    <!-- Phần social media icons -->
                                                    <tr>
                                                        <td align=""center"">
                                                            <table
                                                                style=""border-top:1px solid #3e3e3e;border-bottom:1px solid #3e3e3e;max-width:600px;width:100%""
                                                                cellspacing=""0"" cellpadding=""0"" align=""center"">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align=""center"">
                                                                            <table cellspacing=""0"" cellpadding=""0""
                                                                                border=""0""
                                                                                style=""max-width:600px;width:100%"">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table
                                                                                                style=""background:#000000""
                                                                                                cellspacing=""0""
                                                                                                cellpadding=""0""
                                                                                                border=""0"" width=""100%"">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <td style=""padding:5px 0px""
                                                                                                            align=""center"">
                                                                                                            <table
                                                                                                                cellspacing=""0""
                                                                                                                cellpadding=""0""
                                                                                                                border=""0""
                                                                                                                align=""center"">
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td
                                                                                                                            style=""line-height:10px;font-size:10px"">
                                                                                                                            &nbsp;
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style=""background:transparent;padding-top:5px;padding-bottom:5px""
                                                                                                                            align=""center"">
                                                                                                                            <table
                                                                                                                                cellspacing=""0""
                                                                                                                                cellpadding=""0""
                                                                                                                                border=""0""
                                                                                                                                align=""center"">
                                                                                                                                <tbody>
                                                                                                                                    <tr>
                                                                                                                                        <td
                                                                                                                                            style=""width:26px"">
                                                                                                                                            &nbsp;
                                                                                                                                        </td>
                                                                                                                                        <td style=""width:32px""
                                                                                                                                            align=""center"">
                                                                                                                                            <a href=""https://www.facebook.com/?locale=vi_VN""
                                                                                                                                                alt=""facebook""
                                                                                                                                                style=""text-decoration:none""
                                                                                                                                                target=""_blank""
                                                                                                                                                data-saferedirecturl=""https://www.facebook.com/?locale=vi_VN"">
                                                                                                                                                <img src=""https://ci3.googleusercontent.com/meips/ADKq_NaPBRFT3ub8EENYJe542Vi62ubs8K5zI0TcuolOz-MdfopDr5wJiAN_xIHwudFJx3tqUlOG9zbcgn4Tz6hUdFvrj7Ll577GkaDr92QBVZbE7OjIUo4jiGXlIvRivMi6h8O49V95-DWdHAGKTFyjMGAK94_NYZmYhqfPH7kSpQ=s0-d-e1-ft#http://image.email.amctheatres.com/lib/fe5915707c61017a771d/m/7/fb162d53-978c-49a0-bcde-135f99bad7db.png""
                                                                                                                                                    title=""facebook""
                                                                                                                                                    style=""width:32px;height:32px""
                                                                                                                                                    width=""32""
                                                                                                                                                    height=""32""
                                                                                                                                                    border=""0""
                                                                                                                                                    class=""CToWUd""
                                                                                                                                                    data-bit=""iit"">
                                                                                                                                            </a>
                                                                                                                                        </td>
                                                                                                                                        <td
                                                                                                                                            style=""width:26px"">
                                                                                                                                            &nbsp;
                                                                                                                                        </td>
                                                                                                                                        <td style=""width:32px""
                                                                                                                                            align=""center"">
                                                                                                                                            <a href=""https://www.instagram.com/""
                                                                                                                                                alt=""instagram""
                                                                                                                                                style=""text-decoration:none""
                                                                                                                                                target=""_blank""
                                                                                                                                                data-saferedirecturl=""https://www.instagram.com/"">
                                                                                                                                                <img src=""https://ci3.googleusercontent.com/meips/ADKq_NbmBQ5g1zAEithX3h1aGEIR-QzgFGJZwvHZ-nDtYOmG4AJd5zPtqS-69Tb3PwzitT2Bjd1tWv670GOzk2cq3y_rs29UHl2gc1kYxMDzKo47oy13ondxEeS81IyM8jv_WoMoaq0LXEqa-0R17JobCM_uVL18TkAkzi9mtHOJWg=s0-d-e1-ft#http://image.email.amctheatres.com/lib/fe5915707c61017a771d/m/7/6af4b936-3775-458e-9790-a5aab205670f.png""
                                                                                                                                                    title=""instagram""
                                                                                                                                                    style=""width:32px;height:32px""
                                                                                                                                                    width=""32""
                                                                                                                                                    height=""32""
                                                                                                                                                    border=""0""
                                                                                                                                                    class=""CToWUd""
                                                                                                                                                    data-bit=""iit"">
                                                                                                                                            </a>
                                                                                                                                        </td>
                                                                                                                                        <td
                                                                                                                                            style=""width:26px"">
                                                                                                                                            &nbsp;
                                                                                                                                        </td>
                                                                                                                                        <td style=""width:32""
                                                                                                                                            align=""center"">
                                                                                                                                            <a href=""https://x.com/""
                                                                                                                                                alt=""twitter""
                                                                                                                                                style=""text-decoration:none""
                                                                                                                                                target=""_blank""
                                                                                                                                                data-saferedirecturl=""https://x.com/"">
                                                                                                                                                <img src=""https://ci3.googleusercontent.com/meips/ADKq_Nao-MrHjLyA0VsuaaiqItyWUK3p59ti5saPSG6hNnx-1t5FSN-i6wGxSpDBT5sDzMgalidGrnuGeAPkIt8R9HuWoZ4WhRiRHI_yn4j4Evc4WxTeyQQaczunHADF0EZYWJ5gWb6noAGGG2gou2Ndlm5ZpLAL9QpxrAop0lIacNM=s0-d-e1-ft#https://image.email.amctheatres.com/lib/fe5915707c61017a771d/m/1/be41f356-6977-42f1-a8b1-194c7b57f5ec.png""
                                                                                                                                                    title=""twitter""
                                                                                                                                                    style=""width:32px;height:32px""
                                                                                                                                                    width=""32""
                                                                                                                                                    height=""32""
                                                                                                                                                    border=""0""
                                                                                                                                                    class=""CToWUd""
                                                                                                                                                    data-bit=""iit"">
                                                                                                                                            </a>
                                                                                                                                        </td>
                                                                                                                                        <td
                                                                                                                                            style=""width:26px"">
                                                                                                                                            &nbsp;
                                                                                                                                        </td>
                                                                                                                                        <td style=""width:32px""
                                                                                                                                            align=""center"">
                                                                                                                                            <a href=""https://www.youtube.com/""
                                                                                                                                                alt=""youtube""
                                                                                                                                                style=""text-decoration:none""
                                                                                                                                                target=""_blank""
                                                                                                                                                data-saferedirecturl=""https://www.youtube.com/"">
                                                                                                                                                <img src=""https://ci3.googleusercontent.com/meips/ADKq_NZxUdQ7DHCMbQEZpdqg4LeWOn620St5QUoi49WZs1AASBarptvySn0I1s5i99V7MuQ1bDTCMMy6QWwTGbB2tdQwwLyDz-OtS_CcqKT0gP4aolELuxWpCTWCeEnb5Bsns2bVSQ2YHGhun4TX4SoqkCTEsOWA96v_ZoVKW5SU5w=s0-d-e1-ft#http://image.email.amctheatres.com/lib/fe5915707c61017a771d/m/7/69721c7f-40f3-43f1-9f02-152aeb276291.png""
                                                                                                                                                    title=""youtube""
                                                                                                                                                    style=""width:32px;height:32px""
                                                                                                                                                    width=""32""
                                                                                                                                                    height=""32""
                                                                                                                                                    border=""0""
                                                                                                                                                    class=""CToWUd""
                                                                                                                                                    data-bit=""iit"">
                                                                                                                                            </a>
                                                                                                                                        </td>
                                                                                                                                        <td
                                                                                                                                            style=""width:26px"">
                                                                                                                                            &nbsp;
                                                                                                                                        </td>
                                                                                                                                        <td style=""width:32px""
                                                                                                                                            align=""center"">
                                                                                                                                            <a href=""https://www.tiktok.com/""
                                                                                                                                                alt=""TikTok""
                                                                                                                                                style=""text-decoration:none""
                                                                                                                                                target=""_blank""
                                                                                                                                                data-saferedirecturl=""https://www.tiktok.com/"">
                                                                                                                                                <img src=""https://ci3.googleusercontent.com/meips/ADKq_NapedqAFoWslbaCHBsDhNkIsmg9kR2qCZ4cxMJbWgam4Xk9WTLeeo6Cy3T9L54RhWTPRLx5tIpix8XR0uBM2il1TzOW7pscX5sAP0ZDLJBAsN6hqOMcY-AphJrELiwmx-5AaJNlkVFgL0jK87j0f_396flm_bD8DFE6X0fyqDmX=s0-d-e1-ft#https://image.email.amctheatres.com/lib/fe5915707c61017a771d/m/14/b243d8ac-2704-4735-823d-56a3acaf0d16.png""
                                                                                                                                                    title=""TikTok""
                                                                                                                                                    style=""width:32px;height:32px""
                                                                                                                                                    width=""32""
                                                                                                                                                    height=""32""
                                                                                                                                                    border=""0""
                                                                                                                                                    class=""CToWUd""
                                                                                                                                                    data-bit=""iit"">
                                                                                                                                            </a>
                                                                                                                                        </td>
                                                                                                                                        <td
                                                                                                                                            style=""width:26px"">
                                                                                                                                            &nbsp;
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </tbody>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td
                                                                                                                            style=""line-height:10px;font-size:10px"">
                                                                                                                            &nbsp;
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <!-- Phần thông tin liên hệ -->
                                                    <tr>
                                                        <td>
                                                            <table width=""100%""
                                                                style=""background:rgb(23,23,23);max-width:600px""
                                                                border=""0"" cellspacing=""0"" cellpadding=""0"">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style=""background:rgb(23,23,23)"">
                                                                            <table align=""center"" cellspacing=""0""
                                                                                cellpadding=""0""
                                                                                style=""max-width:600px;width:100%"">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td align=""center""
                                                                                            style=""background:rgb(23,23,23);padding:20px 10px 15px;color:rgb(255,255,255);text-transform:none;line-height:14px;font-family:'gordita',arial,helvetica,sans-serif!important;font-size:11px;font-weight:normal"">
                                                                                            This email was sent by:
                                                                                            <span
                                                                                                class=""m_-269591128180421385applelinksWhite"">CineX
                                                                                                Theatres, Trinh Van Bo
                                                                                                Street, Nam Tu Liem, Ha
                                                                                                Noi</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <!-- Phần liên kết và copyright -->
                                                    <tr>
                                                        <td align=""center""
                                                            style=""max-width:600px;width:100%;border-top:2px solid #4a4a4a"">
                                                            <table role=""presentation"" width=""100%"" cellspacing=""0""
                                                                cellpadding=""0"" border=""0"" style=""max-width:600px"">
                                                                <tbody>
                                                                    <tr>
                                                                        <td
                                                                            style=""padding:20px 0;background-color:#171717"">
                                                                            <table role=""presentation"" align=""center""
                                                                                width=""100%"" border=""0"" cellspacing=""0""
                                                                                cellpadding=""0"" style=""max-width:600px"">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class=""m_-269591128180421385drop-sub""
                                                                                            align=""center""
                                                                                            style=""padding:10px 0"">
                                                                                            <table
                                                                                                class=""m_-269591128180421385drop-sub""
                                                                                                role=""presentation""
                                                                                                cellspacing=""0""
                                                                                                cellpadding=""0""
                                                                                                border=""0"">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <td class=""m_-269591128180421385hide""
                                                                                                            align=""center""
                                                                                                            style=""width:50px;font-family:arial;font-size:11px;color:#000000"">
                                                                                                            &nbsp;</td>
                                                                                                        <td class=""m_-269591128180421385drop-block""
                                                                                                            align=""center""
                                                                                                            style=""font-family:arial;font-size:12px;color:#ffffff"">
                                                                                                            <a href=""http://localhost:4200/""
                                                                                                                alt=""Contact Us""
                                                                                                                style=""text-decoration:none;color:#ffffff""
                                                                                                                target=""_blank""
                                                                                                                data-saferedirecturl=""http://localhost:4200/"">
                                                                                                                Liên Hệ
                                                                                                            </a>
                                                                                                        </td>
                                                                                                        <td class=""m_-269591128180421385hide""
                                                                                                            align=""center""
                                                                                                            style=""width:50px;font-family:arial;font-size:11px;color:#ffffff"">
                                                                                                            &nbsp;|&nbsp;
                                                                                                        </td>
                                                                                                        <td class=""m_-269591128180421385drop-block""
                                                                                                            align=""center""
                                                                                                            style=""font-family:arial;font-size:12px;color:#ffffff"">
                                                                                                            <a href=""http://localhost:4200/trogiup""
                                                                                                                alt=""Privacy Policy""
                                                                                                                style=""text-decoration:none;color:#ffffff""
                                                                                                                target=""_blank""
                                                                                                                data-saferedirecturl=""http://localhost:4200/trogiup"">
                                                                                                                Chính
                                                                                                                Sách
                                                                                                            </a>
                                                                                                        </td>
                                                                                                        <td class=""m_-269591128180421385hide""
                                                                                                            align=""center""
                                                                                                            style=""width:50px;font-family:arial;font-size:11px;color:#000000"">
                                                                                                            &nbsp;</td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align=""center""
                                                                            style=""padding:20px 0;background-color:#000000"">
                                                                            <table class=""m_-269591128180421385drop-sub""
                                                                                role=""presentation"" cellspacing=""0""
                                                                                cellpadding=""0"" border=""0"">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class=""m_-269591128180421385drop-block""
                                                                                            align=""center""
                                                                                            style=""font-family:arial;font-size:12px;color:#ffffff;text-transform:uppercase"">
                                                                                            ©&nbsp;2025 CineX
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        </td>
                        </tr>
                        </tbody>
                        </table>
                    </div><img width=""1"" height=""1"" alt=""""
                        src=""https://ci3.googleusercontent.com/meips/ADKq_NYbYZFXJvu3L_3x-BUifpK1CKmAvWOwxMS5vskfTQIVXadE6mMdc6CQrlHbfWmK9KBmK4V9npATWljMZN56OrTly5IrhX7ihRPVOQjnchkTfoS4-peklJZzn4q6iD5yQbqIo01yj5ffuUf9gNPskgHCkfWr1zcgRsKHehLsvqXtik2oeAJHDiyzJ-F_8OjdRyI8szDlfPPluFKY1Cwxbfs5R_Azxaab-YxzdOfQZk4IdBhzmR0dP_poJho5uLRvUs70r3YNRpjBePcfwu_AeTI3CBXZXpoZc0bXWG8kiGW8jj5Zp7Vsi9DGTQc993zNlM3JzzFy=s0-d-e1-ft#https://email.account.monica.im/o/eJwEwFFuwyAMANDTlL8h4xibfHAYapwFqUCVkJ1_r-YNMR07OstBiDChkLgzJ4qmArJzocp2GCSTajEovxWYXcsIGIECBIg7iK_Eh2ysmCToVtOLoKjOZyzf52hafOvuymezMc4yKPCL4LeX9vE6u1v5ft63Xu272hw_Ovv3Y8vcX8b_AAAA__-FvTFD""
                        class=""CToWUd"" data-bit=""iit"">
                </div>
                <div class=""yj6qo""></div>
                <div class=""adL"">
                </div>
            </div>
        </div>
        <div class=""WhmR8e"" data-hash=""0""></div>
    </div>
</body>

</html>";
            using (MailMessage mail = new MailMessage())
            {
                mail.To.Add(email.Trim());
                mail.From = new MailAddress("thaothaobatbai123@gmail.com", "Hệ thống hỗ trợ");
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;
                mail.BodyEncoding = Encoding.UTF8;

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
    }

}
