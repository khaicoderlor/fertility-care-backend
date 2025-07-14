using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Statistics
{
    public class AverageRateMonthly
    {
        public decimal Rating {  get; set; }
        public decimal Monthly { get; set; }
        public bool IsData {  get; set; }
    }
}
