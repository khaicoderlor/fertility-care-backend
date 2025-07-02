using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.OrderSteps
{
    public class OrderStepPaymentTuple
    {
        public string TreatmentName { get; set; }   

        public string DoctorName { get; set; }  

        public string OrderStepName { get; set; }

        public int StepOrder { get; set; }

        public string? PaymentCode { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentDate { get; set; }

        public string Status { get; set; }

        public string? GatewayMessage { get; set; }

    }
}
