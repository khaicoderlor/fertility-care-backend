using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class CreateFeedbackRequestDTO
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string TreatmentServiceId { get; set; }
        public decimal Rating { get; set; }
        public string? Comment { get; set; }


    }
}
