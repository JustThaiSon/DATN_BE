using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Counter.Res
{
    public class PaymentHistoryRes
    {
        public string TransactionCode { get; set; }
        public long AmountPaid { get; set; }
        public string FormattedAmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string FormattedPaymentDate { get; set; }
        public string PaymentMethodName { get; set; }
        public string LogoUrl { get; set; }
    }
}
