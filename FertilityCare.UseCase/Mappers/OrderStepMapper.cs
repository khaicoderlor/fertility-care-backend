using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.OrderSteps;
using FertilityCare.UseCase.DTOs.TreatmentStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Mappers
{
    public static class OrderStepMapper
    {

        public static OrderStepDTO MapToStepDTO(this OrderStep orderStep)
        {
            return new OrderStepDTO
            {
                Id = orderStep.Id,
                TreatmentStep = orderStep.TreatmentStep.MapToTreatmentStepDTO(),
                Note = orderStep.Note,
                Status = orderStep.Status.ToString(),
                StartDate = orderStep.StartDate.ToString("dd/MM/yyyy"),
                EndDate = orderStep.EndDate?.ToString("dd/MM/yyyy"),
                PaymentStatus = orderStep.PaymentStatus.ToString(),
                TotalAmount = orderStep.TotalAmount,
                Appointments = orderStep.Appointments.Select(x => x.MapToAppointmentFollowStep()).ToList(),
                CreatedAt = orderStep.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                UpdatedAt = orderStep.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }

    }
}
