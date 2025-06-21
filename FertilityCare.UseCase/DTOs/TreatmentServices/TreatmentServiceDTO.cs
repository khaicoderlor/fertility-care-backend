using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.TreatmentStep;

namespace FertilityCare.UseCase.DTOs.TreatmentServices
{
    public class TreatmentServiceDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal? EstimatePrice { get; set; }

        public int? Duration { get; set; }

        public decimal? SuccessRate { get; set; }

        public string? RecommendedFor { get; set; }

        public string? Contraindications { get; set; }

        public string CreatedAt { get; set; }  

        public string? UpdatedAt { get; set; }

        public List<TreatmentStepDTO>? TreatmentSteps { get; set; }
    }
}
