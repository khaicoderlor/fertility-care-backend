using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.PrescriptionItem;

namespace FertilityCare.UseCase.DTOs.Prescriptions
{
    public class CreatePrecriptionRequestDTO
    {
        public string OrderId { get; set; }
        public List<CreatePrescriptionItem>? PrescriptionItems { get; set; }
    }
}
