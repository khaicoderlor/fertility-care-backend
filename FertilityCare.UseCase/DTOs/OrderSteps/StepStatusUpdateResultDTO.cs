using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.OrderSteps
{
    public class StepStatusUpdateResultDTO
    {
        public OrderStepDTO Step { get; set; }
        public string NextStepStatus { get; set; }
    }
}
