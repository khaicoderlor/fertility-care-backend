using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.EmbryoTransfers;

namespace FertilityCare.UseCase.Mappers
{
    public static class EmbryoTranferMapper
    {
        public static EmbryoTransferDTO MapToEmbryoTranferDTO(this EmbryoTransfer embryoTranfer)
        {
            return new EmbryoTransferDTO()
            {
                EmbryoTransferId = embryoTranfer.Id,
                EmbryoGainedId = embryoTranfer.EmbryoGainedId,
                Appointment = embryoTranfer.Appointment?.Id.ToString(),
                TransferDate = embryoTranfer.TransferDate.ToString("yyyy-MM-dd"),
                UpdatedAt = embryoTranfer.UpdatedAt?.ToString("yyyy-MM-dd"),
                TransferType = embryoTranfer.TransferType.ToString(),
                OrderId = embryoTranfer.OrderId.ToString()
            };

        }
    }
}
