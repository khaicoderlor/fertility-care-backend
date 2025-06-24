using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EmbryoGained
{
    public class CreateEmbryoGainedRequestDTO
    {
        public long EggGainedId { get; set; }      
        public string Grade { get; set; }     
        public bool IsViable { get; set; }         
    }
}
