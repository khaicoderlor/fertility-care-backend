using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.PrescriptionItem;
using FertilityCare.UseCase.DTOs.Prescriptions;

namespace FertilityCare.UseCase.Mappers
{
    public static class PrescriptionMapper
    {
        public static PrescriptionDTO MapToPrescriptionDTO(this Domain.Entities.Prescription prescription)
        {
            return new PrescriptionDTO()
            {
                Id = prescription.Id.ToString(),
                OrderId = prescription.OrderId.ToString(),
                PrescriptionDate = prescription.PrescriptionDate.ToString("yyyy-MM-dd"),
                Note = prescription.Note,
                PrescriptionItems = prescription.PrescriptionItems?.Select(item => item.MapToPrescriptionItemDTO()).ToList()
            };
        }
        public static Prescription MapToPrescription(this PrescriptionDTO prescriptionDTO)
        {
            return new Prescription()
            {
                Id = Guid.Parse(prescriptionDTO.Id),
                OrderId = Guid.Parse(prescriptionDTO.OrderId),
                PrescriptionDate = DateTime.Parse(prescriptionDTO.PrescriptionDate ?? DateTime.Now.ToString("yyyy-MM-dd")),
                Note = prescriptionDTO.Note,
                PrescriptionItems = prescriptionDTO.PrescriptionItems?.Select(item => item.MapToPrescriptionItem()).ToList() ?? new List<PrescriptionItem>()
            };
        }

    }
}
