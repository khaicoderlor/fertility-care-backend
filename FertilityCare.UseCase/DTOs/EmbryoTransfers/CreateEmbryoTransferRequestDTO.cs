using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EmbryoTransfers
{
    public class CreateEmbryoTransferRequestDTO
    {
        public long EmbryoId { get; set; }
        public string AppointmentId { get; set; }
        public string OrderId { get; set; }
    }
}
