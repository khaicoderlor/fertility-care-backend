using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EmbryoTransfers
{
    public class EmbryoTransferredReportResponse
    {
        public long Id { get; set; }

        public long EmbryoId { get; set; }
        
        public string EmbryoGrade { get; set; }

        public string TransferDate { get; set; }

        public string TransferType { get; set; }

        public string OrderId { get; set; }

    }
}
