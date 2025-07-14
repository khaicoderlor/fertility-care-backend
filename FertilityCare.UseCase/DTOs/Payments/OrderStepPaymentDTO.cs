using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.OrderSteps;
using FertilityCare.UseCase.DTOs.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FertilityCare.UseCase.DTOs.Payments
{
    public class OrderStepPaymentDTO
    {
        public string Id { get; set; } = string.Empty;

        public PatientDTO Patient { get; set; } = new PatientDTO();

        public string TreatmentServiceName { get; set; } = string.Empty;

        public OrderStepDTO OrderStep { get; set; } = new OrderStepDTO();

        public string PaymentCode { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string TransactionCode { get; set; } = string.Empty;

        public string PaymentDate { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;

        public string GatewayResponseCode { get; set; } = string.Empty; 

        public string GatewayMessage { get; set; } = string.Empty;

    }
}
