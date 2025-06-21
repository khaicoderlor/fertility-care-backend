using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class FeedbackDTO
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string? DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? TreatmentServiceId { get; set; }
        public string? TreatmentServiceName { get; set; }
        public bool Status { get; set; }
        public decimal Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
