using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.PrescriptionItem;

namespace FertilityCare.UseCase.DTOs.Prescriptions
{
    public class PrescriptionDTO
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string? PrescriptionDate { get; set; }
        public string? Note { get; set; }
        public List<PrescriptionItemDTO>? PrescriptionItems { get; set; } 
    }
}
