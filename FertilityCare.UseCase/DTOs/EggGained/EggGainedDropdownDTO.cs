using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EggGained
{
    public class EggGainedDropdownDTO
    {
        public long Id { get; set; }
        public EggGrade Grade { get; set; }
        public DateOnly DateGained { get; set; }
    }
}
