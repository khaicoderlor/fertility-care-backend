using FertilityCare.UseCase.DTOs.EggGained;
using FertilityCare.UseCase.DTOs.Embryos;
using FertilityCare.UseCase.DTOs.EmbryoTransfers;
using FertilityCare.UseCase.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Statistics
{
    public class ReportProgressSideAdmin
    {
        public List<EggReportResponse> eggs { get; set; }

        public List<EmbryoReportResponse> embryos { get; set; }

        public List<EmbryoTransferredReportResponse> embryosTransferred { get; set; }

    }
}
