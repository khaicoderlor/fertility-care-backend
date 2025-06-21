using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Payments
{
    public class MomoPaymentRequest
    {
        public string PartnerCode { get; set; }
        public string AccessKey { get; set; }
        public string RequestId { get; set; }
        public decimal Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string RedirectUrl { get; set; }
        public string NotifyUrl { get; set; }
        public string ExtraData { get; set; } = "";
        public string RequestType { get; set; }
        public string Signature { get; set; }
        public string Lang { get; set; } = "vi";
    }

}
