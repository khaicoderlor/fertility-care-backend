using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class FeedbackQueryDTO
    {
        public string? PatientId { get; set; }
        public string? DoctorId { get; set; }
        public string? TreatmentServiceId { get; set; }
        public decimal? rating { get; set; } 
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
