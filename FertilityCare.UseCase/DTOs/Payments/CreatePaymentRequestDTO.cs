using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Payments
{
    public class CreatePaymentRequestDTO
    {

        public Guid PatientId { get; set; }

        public long OrderStepId { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string TreatmentName { get; set; }

        public string? OrderInfo { get; set; }

        public string? ExtraData { get; set; } 
    }

}
