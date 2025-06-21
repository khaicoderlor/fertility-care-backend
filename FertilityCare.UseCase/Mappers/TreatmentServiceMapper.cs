using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using FertilityCare.UseCase.DTOs.TreatmentStep;

namespace FertilityCare.UseCase.Mappers
{
    public static class TreatmentServiceMapper
    {
        public static TreatmentServiceDTO MapToTreatmentServiceDTO(this TreatmentService treatmentServices)
        {
            return new TreatmentServiceDTO
            {
                Id = treatmentServices.Id.ToString(),
                Name = treatmentServices.Name,
                Description = treatmentServices.Description,
                Duration = treatmentServices.Duration,
                SuccessRate = treatmentServices.SuccessRate,
                RecommendedFor = treatmentServices.RecommendedFor,
                Contraindications = treatmentServices.Contraindications,
                UpdatedAt = treatmentServices.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss"),
                TreatmentSteps = treatmentServices.TreatmentSteps?.Select(x => x.MapToTreatmentStepDTO()).ToList(),
                EstimatePrice = treatmentServices.EstimatePrice,
            };
        }
        public static TreatmentService MapToTreatmentService(this TreatmentServiceDTO treatmentServiceDTO)
        {
            return new TreatmentService
            {
                Id = Guid.Parse(treatmentServiceDTO.Id),
                Name = treatmentServiceDTO.Name,
                Description = treatmentServiceDTO.Description,
                Duration = treatmentServiceDTO.Duration,
                SuccessRate = treatmentServiceDTO.SuccessRate,
                RecommendedFor = treatmentServiceDTO.RecommendedFor,
                Contraindications = treatmentServiceDTO.Contraindications,
                UpdatedAt = string.IsNullOrEmpty(treatmentServiceDTO.UpdatedAt) ? null : DateTime.Parse(treatmentServiceDTO.UpdatedAt),
                EstimatePrice = treatmentServiceDTO.EstimatePrice,
                TreatmentSteps = treatmentServiceDTO.TreatmentSteps?.Select(x => x.MapToTreatmentStep()).ToList(),
            };
        }
    }
}
