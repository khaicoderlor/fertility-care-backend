using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class TreatmentService
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal? EstimatePrice { get; set; }
    
    public int? Duration { get; set; } 

    public decimal? SuccessRate { get; set; }

    public string? RecommendedFor { get; set; }

    public string? Contraindications { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual List<TreatmentStep>? TreatmentSteps { get; set; }


}
