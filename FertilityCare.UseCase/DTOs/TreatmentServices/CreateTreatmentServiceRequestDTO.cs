using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.TreatmentServices
{
    public class CreateTreatmentServiceRequestDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? EstimatePrice { get; set; }

    }
}
