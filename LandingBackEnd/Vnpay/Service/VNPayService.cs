using DATN_BackEndApi.VNPay;
using DATN_LandingPage.Extension.Vnpay;

public interface IVNPayService
{
    string CreatePaymentUrl(OrderInfo order, HttpContext context);
    PaymentResponse ProcessPaymentCallback(IQueryCollection collections);
}

public class VNPayService : IVNPayService
{
    private readonly IConfiguration _configuration;
    private readonly VNPayConfig _options;

    public VNPayService(IConfiguration configuration)
    {
        _configuration = configuration;
        _options = configuration.GetSection(VNPayConfig.ConfigName).Get<VNPayConfig>();
    }

    public string CreatePaymentUrl(OrderInfo order, HttpContext context)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();
        var pay = new VNPayLibrary();

        pay.AddRequestData("vnp_Version", "2.1.0");
        pay.AddRequestData("vnp_Command", "pay");
        pay.AddRequestData("vnp_TmnCode", _options.TmnCode);
        pay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); // Amount in VND
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", "VND");
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", "vn");
        pay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {order.OrderId}");
        pay.AddRequestData("vnp_OrderType", "other");
        //pay.AddRequestData("vnp_ReturnUrl", _options.ReturnUrl);
        //pay.AddRequestData("vnp_TxnRef", tick);


        // Sử dụng URL tạm thời cho vnp_ReturnUrl
        var returnUrl = $"{context.Request.Scheme}://{context.Request.Host}/api/Movie/payment-callback";
        pay.AddRequestData("vnp_ReturnUrl", returnUrl);
        pay.AddRequestData("vnp_TxnRef", tick);



        var paymentUrl = pay.CreateRequestUrl(_options.PaymentUrl, _options.HashSecret);

        return paymentUrl;
    }

    public PaymentResponse ProcessPaymentCallback(IQueryCollection collections)
    {
        var pay = new VNPayLibrary();
        var response = pay.GetFullResponseData(collections, _options.HashSecret);

        return response;
    }
}