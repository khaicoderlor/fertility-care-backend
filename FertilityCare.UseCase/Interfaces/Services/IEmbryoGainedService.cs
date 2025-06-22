using FertilityCare.UseCase.DTOs.EmbryoGained;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IEmbryoGainedService
    {
        Task AddEmbryosAsync(Guid orderId, CreateEmbryoGainedListRequestDTO request);
    }
}
