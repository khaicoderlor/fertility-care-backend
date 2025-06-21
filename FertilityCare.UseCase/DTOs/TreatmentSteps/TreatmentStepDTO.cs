using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;

namespace FertilityCare.UseCase.DTOs.TreatmentStep
{
    public class TreatmentStepDTO
    {
        public string Id { get; set; }

        public string StepName { get; set; }

        public string? Description { get; set; }

        public int StepOrder { get; set; }

        public int? EstimatedDurationDays { get; set; }

        public decimal? Amount { get; set; }

        public string CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }
    }
}
