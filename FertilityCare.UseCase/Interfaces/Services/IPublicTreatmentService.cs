using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IPublicTreatmentService
    {
        Task<IEnumerable<TreatmentServiceDTO>> GetAllAsync();
        Task<TreatmentServiceDTO> UpdateAsync(TreatmentServiceDTO treatmentServiceDTO);
        Task<TreatmentStep> UpdateStepAsync(string id, TreatmentStepUpdateDTO dto);

    }
}
