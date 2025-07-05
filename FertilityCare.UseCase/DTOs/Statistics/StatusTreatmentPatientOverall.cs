using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Statistics
{
    public class StatusTreatmentPatientOverall
    {
        public string Name { get; set; } = string.Empty;
        
        public long Value { get; set; }

        public string Color { get; set; } = string.Empty;   
    }
}

