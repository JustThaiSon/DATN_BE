using DATN_BackEndApi.Extension.Vnpay.DATN_BackEndApi.VNPay;
using DATN_Helpers.Common;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVNPayService _vnPayService;
        private readonly string _langCode;

        public PaymentController(IVNPayService vnPayService, IConfiguration configuration)
        {
            _vnPayService = vnPayService;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
        }

        [HttpPost]
        [Route("create-payment")]
        public async Task<CommonResponse<string>> CreatePayment([FromBody] OrderInfo orderInfo)
        {
            var res = new CommonResponse<string>();
            try
            {
                var paymentUrl = _vnPayService.CreatePaymentUrl(orderInfo, HttpContext);
                res.Data = paymentUrl;
                res.ResponseCode = 1;
                res.Message = "Success";
            }
            catch (Exception ex)
            {
                res.ResponseCode = -99;
                res.Message = ex.Message;
            }
            return res;
        }

        [HttpGet]
        [Route("payment-callback")]
        public IActionResult PaymentCallback()
        {
            //var res = new CommonResponse<PaymentResponse>();
            //try
            //{
            //    var response = _vnPayService.ProcessPaymentCallback(Request.Query);
            //    res.Data = response;
            //    res.ResponseCode = 1;
            //    res.Message = "Success";
            //}
            //catch (Exception ex)
            //{
            //    res.ResponseCode = -99;
            //    res.Message = ex.Message;
            //}
            //return res;


            var response = _vnPayService.ProcessPaymentCallback(Request.Query);

            // Redirect back to Angular with payment result
            var redirectUrl = "http://localhost:4200/payment-callback";
            var queryString = $"?vnp_ResponseCode={response.VnPayResponseCode}&vnp_TxnRef={response.OrderId}";

            return Redirect(redirectUrl + queryString);
        }



    }
}