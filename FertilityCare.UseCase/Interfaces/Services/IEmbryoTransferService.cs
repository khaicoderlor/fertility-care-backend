using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.EmbryoTransfers;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IEmbryoTransferService
    {
        Task<EmbryoTransferDTO> CreateEmbryoTransferAsync(CreateEmbryoTransferRequestDTO request, bool isFrozen);
        Task<bool> ReTransferAsync(CreateEmbryoReTransferRequestDTO request);
    }
}
