using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.PrescriptionItem;
using FertilityCare.UseCase.DTOs.Prescriptions;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IPrescriptionService
    {
        Task<PrescriptionDTO> CreatePrescriptionAsync(CreatePrecriptionRequestDTO request);
        Task<IEnumerable<PrescriptionDTO>> FindPrescriptionByOrderIdAsync(string orderId);
        Task<PrescriptionDTO> AddPrescriptionItemToPrescriptionAsync(PrescriptionItemDTO prescriptionItem, string prescriptionId);

        Task<IEnumerable<PrescriptionDetailDTO>> GetPrescriptionByPatientId(string patientId);
    }
}
