using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.EggGained;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IEggGainedService
    {
        Task<CreateEggResponseDTO> AddEggsAsync(Guid orderId, CreateEggGainedListRequestDTO request);

        Task<IEnumerable<EmbryoDropdownEggDTO>> GetUsableEggsByOrderIdAsync(Guid orderId);
    }
}
