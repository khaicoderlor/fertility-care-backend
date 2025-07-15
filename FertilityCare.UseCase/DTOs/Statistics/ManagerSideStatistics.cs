using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Statistics
{
    public class ManagerSideStatistics
    {
        public int TotalPatients { get; set; }
        public int TotalInProgressOrder { get; set; }
        public int TotalCompleteOrder { get; set; }
        public int TotalPlannedOrder { get; set; }
    }
}
