using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Embryos
{
    public class EmbryoReportResponse
    {
        public long Id { get; set; }

        public string EmbryoGrade { get; set; }

        public string EggGrade { get; set; }

        public long EggId { get; set; }

        public string EmbryoStatus { get; set; }

        public bool IsViable { get; set; }

        public bool IsFrozen { get; set; }

        public bool IsTransferred { get; set; }

        public string OrderId { get; set; }


    }
}