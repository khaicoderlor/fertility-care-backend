using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EggGained
{
    public class EggReportResponse
    {
        public long Id { get; set; }

        public string Grade { get; set; }

        public bool IsUsable { get; set; }

        public string DateGain { get; set; }

        public string OrderId { get; set; }

    }
}
