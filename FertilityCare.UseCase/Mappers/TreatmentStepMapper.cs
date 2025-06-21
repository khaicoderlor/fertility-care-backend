using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.TreatmentStep;

namespace FertilityCare.UseCase.Mappers
{
    public static class TreatmentStepMapper
    {
        public static TreatmentStepDTO MapToTreatmentStepDTO(this TreatmentStep service)
        {
            return new TreatmentStepDTO()
            {
                Id = service.Id.ToString(),
                StepName = service.StepName,
                Description = service.Description,
                StepOrder = service.StepOrder,
                EstimatedDurationDays = service.EstimatedDurationDays,
                Amount = service.Amount,
                UpdatedAt = service.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss"),
            };
        }

        public static TreatmentStep MapToTreatmentStep(this TreatmentStepDTO serviceDTO)
        {
            return new TreatmentStep()
            {
                Id = long.Parse(serviceDTO.Id),
                StepName = serviceDTO.StepName,
                Description = serviceDTO.Description,
                StepOrder = serviceDTO.StepOrder,
                EstimatedDurationDays = serviceDTO.EstimatedDurationDays,
                Amount = serviceDTO.Amount,
                UpdatedAt = string.IsNullOrEmpty(serviceDTO.UpdatedAt) ? null : DateTime.Parse(serviceDTO.UpdatedAt),
            };
        }
    }
}