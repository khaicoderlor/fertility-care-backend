using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Embryos
{
    public class EmbryoData
    {
        public string EmbryoId { get; set; }

        public string EmbryoGrade { get; set; }
        public string EggGrade { get; set; }

        public string Status { get; set; }
        public bool IsFrozen { get; set; }
    }
}
