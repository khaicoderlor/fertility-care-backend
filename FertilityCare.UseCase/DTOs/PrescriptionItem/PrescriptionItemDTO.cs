using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;

namespace FertilityCare.UseCase.DTOs.PrescriptionItem
{
    public class PrescriptionItemDTO
    {
        public long Id { get; set; }
        public string PrescriptionId { get; set; }
        public string? MedicationName { get; set; }
        public int? Quantity { get; set; }
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? SpecialInstructions { get; set; } 
    }
}

