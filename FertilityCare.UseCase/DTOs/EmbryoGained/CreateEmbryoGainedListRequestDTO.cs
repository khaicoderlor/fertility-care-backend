using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EmbryoGained
{
    public class CreateEmbryoGainedListRequestDTO
    {
        public List<CreateEmbryoGainedRequestDTO> Embryos { get; set; }
    }
}
