using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.TreatmentServices
{
    public class TreatmentStepUpdateDTO
    {
        public string StepName { get; set; } = default!;
        public string? Description { get; set; }
        public int StepOrder { get; set; }
        public int? EstimatedDurationDays { get; set; }
        public decimal? Amount { get; set; }
    }

}
