using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class UpdateFeedbackDTO
    {
        public decimal Rating { get; set; }
        public string? Comment { get; set; }

    }
}
