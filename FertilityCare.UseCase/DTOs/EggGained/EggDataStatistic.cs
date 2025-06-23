using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EggGained
{
    public class EggDataStatistic
    {
        public string Grade { get; set; } = string.Empty;

        public long Quantity { get; set; } = 0;

        public long ViableCount { get; set; } = 0;
    }
}
