using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.TreatmentStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.OrderSteps
{
    public class OrderStepDTO
    {
        public long Id { get; set; }

        public TreatmentStepDTO? TreatmentStep { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }

        public string? StartDate { get; set; }

        public string? PaymentStatus { get; set; }

        public decimal? TotalAmount { get; set; }

        public List<AppointmentFollowStep> Appointments { get; set; } = new List<AppointmentFollowStep>();

        public string? EndDate { get; set; }

        public string? CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }

    }
}
