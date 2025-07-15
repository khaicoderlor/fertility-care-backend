using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.EggGained;
using FertilityCare.UseCase.DTOs.Embryos;
using FertilityCare.UseCase.DTOs.EmbryoTransfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static EggReportResponse MapToEggReportResponse(this EggGained egg)
        {
            return new EggReportResponse
            {
                Id = egg.Id,
                Grade = egg.Grade.ToString(),
                IsUsable = egg.IsUsable,
                DateGain = egg.DateGained.ToString("yyyy-MM-dd"),
                OrderId = egg.OrderId.ToString()
            };
        }

        public static EmbryoReportResponse MapToEmbryoReportResponse(this EmbryoGained embryo)
        {
            return new EmbryoReportResponse
            {
                Id = embryo.Id,
                EmbryoGrade = embryo.Grade.ToString(),
                EggGrade = embryo.EggGained?.Grade.ToString(),
                EggId = embryo.EggGainedId,
                EmbryoStatus = embryo.EmbryoStatus.ToString(),
                IsViable = embryo.IsViable,
                IsFrozen = embryo.IsFrozen,
                IsTransferred = embryo.IsTransfered,
                OrderId = embryo.OrderId.ToString()
            };
        }

        public static EmbryoTransferredReportResponse MapToEmbryoTransferredReportResponse(this EmbryoTransfer transfer)
        {
            return new EmbryoTransferredReportResponse
            {
                Id = transfer.Id,
                EmbryoId = transfer.EmbryoGainedId,

                // 👇 Truy cập EggId thông qua navigation property
                EggId = transfer.EmbryoGained?.EggGainedId ?? 0,

                EmbryoGrade = transfer.EmbryoGained?.Grade.ToString(),
                TransferDate = transfer.TransferDate.ToString("yyyy-MM-dd"),
                TransferType = transfer.TransferType.ToString(),
                OrderId = transfer.OrderId.ToString()
            };
        }
    }
}
