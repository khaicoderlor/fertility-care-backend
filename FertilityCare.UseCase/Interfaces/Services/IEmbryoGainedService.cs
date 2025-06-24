using FertilityCare.UseCase.DTOs.EmbryoGained;
using FertilityCare.UseCase.DTOs.Embryos;
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
        Task<List<EmbryoData>> GetEmbryosByOrderIdAsync(string orderId);
        Task<IEnumerable<EmbryoReportResponse>> GetEmbryosReportByOrderIdAsync(Guid orderId);
    }
}
