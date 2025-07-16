using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.PrescriptionItem;

namespace FertilityCare.UseCase.Mappers
{
    public static class PrescriptionItemMapper
    {
        public static PrescriptionItemDTO MapToPrescriptionItemDTO(this PrescriptionItem prescriptionItem)
        {
            return new PrescriptionItemDTO()
            {
                Id = prescriptionItem.Id,
                SpecialInstructions = prescriptionItem.SpecialInstructions,
                Quantity = prescriptionItem.Quantity,
                MedicationName = prescriptionItem.MedicationName
            };
        }
        public static PrescriptionItem MapToPrescriptionItem(this PrescriptionItemDTO prescriptionItemDTO)
        {
            return new PrescriptionItem()
            {
                Id = prescriptionItemDTO.Id,
                SpecialInstructions = prescriptionItemDTO.SpecialInstructions ?? "",
                Quantity = prescriptionItemDTO.Quantity ?? 0,
                MedicationName = prescriptionItemDTO.MedicationName
            };
        }
    }
}
