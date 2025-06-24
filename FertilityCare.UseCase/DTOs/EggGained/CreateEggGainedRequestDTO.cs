using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.EggGained
{
    public class CreateEggGainedRequestDTO
    {
        public string Grade { get; set; } = string.Empty;
        public bool isQualified { get; set; } = false;
    }
}
