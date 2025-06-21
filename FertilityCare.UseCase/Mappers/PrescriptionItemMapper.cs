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
                PrescriptionId = prescriptionItem.PrescriptionId.ToString(),
                StartDate = prescriptionItem.StartDate.ToString("dd/MM/yyyy"),
                EndDate = prescriptionItem.EndDate?.ToString("dd/MM/yyyy"),
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
                PrescriptionId = Guid.Parse(prescriptionItemDTO.PrescriptionId),
                StartDate = DateOnly.ParseExact(prescriptionItemDTO.StartDate, "dd/MM/yyyy"),
                EndDate = string.IsNullOrEmpty(prescriptionItemDTO.EndDate) ? null : DateOnly.ParseExact(prescriptionItemDTO.EndDate, "dd/MM/yyyy"),
                SpecialInstructions = prescriptionItemDTO.SpecialInstructions ?? "",
                Quantity = prescriptionItemDTO.Quantity ?? 0,
                MedicationName = prescriptionItemDTO.MedicationName
            };
        }
    }
}
