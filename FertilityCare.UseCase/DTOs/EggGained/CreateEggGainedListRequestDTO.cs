using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EggGained
{
    public class CreateEggGainedListRequestDTO
    {
        public IEnumerable<CreateEggGainedRequestDTO> Eggs { get; set; }
    }
}
