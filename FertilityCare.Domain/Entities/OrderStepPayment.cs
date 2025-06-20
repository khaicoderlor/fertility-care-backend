using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities
{
    public class OrderStepPayment
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public long OrderStepId { get; set; }

        public virtual OrderStep OrderStep { get; set; }

        public string? PaymentCode { get; set; }

        public decimal TotalAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string? TransactionCode { get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentStatus Status { get; set; }

        public string? GatewayResponseCode { get; set; }

        public string? GatewayMessage { get; set; }
    }
}
