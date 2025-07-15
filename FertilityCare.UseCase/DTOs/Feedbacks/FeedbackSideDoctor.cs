using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class FeedbackSideDoctor
    {
        public string Id { get; set; }
        public PatientDTO Patient { get; set; }
        public DoctorDTO? Doctor { get; set; }
        public string PatientEmail { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public TreatmentServiceDTO? TreatmentService { get; set; }
        public bool Status { get; set; }
        public decimal Rating { get; set; }
        public string? Comment { get; set; }
        public string CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
    }
}
