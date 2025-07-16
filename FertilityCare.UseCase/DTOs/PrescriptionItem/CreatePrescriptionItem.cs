using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.PrescriptionItem
{
    public class CreatePrescriptionItem
    {
        public string medicationName { get; set; } = string.Empty;  

        public int quantity { get; set; } = 1;  

        public string? specialInstructions { get; set; } = string.Empty;
    }
}
