using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;

namespace FertilityCare.UseCase.DTOs.EmbryoTransfers
{
    public class EmbryoTransferDTO
    {
        public long EmbryoTransferId { get; set; }
        public long? EmbryoGainedId { get; set; }
        public string? Appointment { get; set; }
        public string? TransferDate { get; set; }
        public string? UpdatedAt { get; set; }
        public string TransferType { get; set; }
        public string? OrderId { get; set; }
    }
}
