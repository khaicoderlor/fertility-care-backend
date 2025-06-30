using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Payments
{
    public record PaymentExecuteResponseDTO
    {
        public string Amount { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string OrderInfo { get; set; } = string.Empty;
        public string ResultCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string ExtraData { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
