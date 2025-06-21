using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.TreatmentServices;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IPublicTreatmentService
    {
        Task<IEnumerable<TreatmentServiceDTO>> GetAllAsync();
        Task<TreatmentServiceDTO> UpdateAsync(TreatmentServiceDTO treatmentServiceDTO);
    }
}
