using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Payments
{
    public class PaymentExecuteResponseDTO
    {
        public string Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string ResultCode { get; set; }
        public string Message { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }
    }
}
