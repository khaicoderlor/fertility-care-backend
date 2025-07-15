using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Statistics
{
    public class RecentStatistics
    {
        public string totalPatients { get; set; } 
        public string totalDoctor { get; set; }
        public string totalOrders { get; set; }
        public string totalRevenue { get; set; }
        public string totalAppointments { get; set; }
        public string totalEggsByMonth { get; set; }
        public string totalEmbryosByMonth { get; set; }
        public string totalEmryoTransfersByMonth { get; set; }
        public string totalRevenueByIVF { get; set; }
        public string totalRevenueByIUI { get; set; }
    }
}
