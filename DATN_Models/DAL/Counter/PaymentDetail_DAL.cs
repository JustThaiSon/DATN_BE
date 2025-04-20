using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Counter
{
    public class PaymentDetail_DAL
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public long AmountPaid { get; set; }
        public string TransactionCode { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethodName { get; set; }
        public string LogoUrl { get; set; }
    }
}
