using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.EmbryoTransfers;
using FertilityCare.UseCase.DTOs.OrderSteps;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IEmbryoTransferService
    {
        Task<EmbryoTransferDTO> CreateEmbryoTransferAsync(CreateEmbryoTransferRequestDTO request);
        Task<IEnumerable<EmbryoTransferredReportResponse>> GetEmbryoTransferReportByOrderIdAsync(Guid guid);
        Task<OrderStepDTO> ReTransferAsync(string orderId);

        Task<bool> MakeStatusOrderIsFrozen(string orderId, bool isFrozen);

    }
}
