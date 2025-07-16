using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.PrescriptionItem
{
    public class CreatePrescriptionItem
    {
        public string MedicationName { get; set; } = string.Empty;  

        public int Quantity { get; set; } = 1;  

        public string? SpecialInstructions { get; set; } = string.Empty;
    }
}
