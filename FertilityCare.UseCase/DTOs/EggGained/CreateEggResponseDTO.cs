using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EggGained
{
    public class CreateEggResponseDTO
    {
        public IEnumerable<EggGainedDropdownDTO> UsableEggs { get; set; }
        public IEnumerable<EggGainedDropdownDTO> UnusableEggs { get; set; }
    }
}
