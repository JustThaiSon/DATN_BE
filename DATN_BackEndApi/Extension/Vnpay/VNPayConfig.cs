using DATN_BackEndApi.VNPay;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DATN_BackEndApi.Extension.Vnpay
{
    public class VNPayLibrary
    {
        private SortedList<string, string> _requestData = new SortedList<string, string>(new VNPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public string GetIpAddress(HttpContext context)
        {
            string ipAddress;
            try
            {
                ipAddress = context.Connection.RemoteIpAddress?.ToString();

                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = "127.0.0.1";
                }
            }
            catch (Exception)
            {
                ipAddress = "127.0.0.1";
            }

            return ipAddress;
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();

            baseUrl += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }

            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }

        public PaymentResponse GetFullResponseData(IQueryCollection collection, string hashSecret)
        {
            var vnpayData = new Dictionary<string, string>();
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(value) && key.StartsWith("vnp_"))
                {
                    vnpayData.Add(key, value);
                }
            }

            PaymentResponse vnpayResponse = new PaymentResponse();
            foreach (var (key, value) in vnpayData)
            {
                switch (key)
                {
                    case "vnp_OrderInfo":
                        vnpayResponse.OrderDescription = value;
                        break;
                    case "vnp_TransactionNo":
                        vnpayResponse.TransactionId = value;
                        break;
                    case "vnp_TxnRef":
                        vnpayResponse.OrderId = value;
                        break;
                    case "vnp_ResponseCode":
                        vnpayResponse.VnPayResponseCode = value;
                        break;
                }
            }

            return vnpayResponse;
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
    }

    public class VNPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }

    public class VNPayConfig
    {
        public static string ConfigName => "VNPay";
        public string Version { get; set; } = "2.1.0";
        public string TmnCode { get; set; } = "C97ZXYVY";
        public string HashSecret { get; set; } = "5QV88EXQ18V2V8473WMNBS3IVOLJFVTW";
        public string PaymentUrl { get; set; } = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        public string ReturnUrl { get; set; } = "http://localhost:4200/payment-callback"; // thay cái này thành 1 route trong angular (hiện tại để tạm thế này đã)
    }


    namespace DATN_BackEndApi.VNPay
    {

        public class OrderInfo
        {
            public Guid OrderId { get; set; }
            public long Amount { get; set; }
            public string OrderDesc { get; set; }
            public DateTime CreatedDate { get; set; }
            public string Status { get; set; }
            public long PaymentTranId { get; set; }
            public string BankCode { get; set; }
            public string PayStatus { get; set; }
        }

        public class PaymentResponse
        {
            public string OrderDescription { get; set; }
            public string TransactionId { get; set; }
            public string OrderId { get; set; }
            public string PaymentMethod { get; set; }
            public string PaymentId { get; set; }
            public bool Success { get; set; }
            public string Token { get; set; }
            public string VnPayResponseCode { get; set; }
        }
    }