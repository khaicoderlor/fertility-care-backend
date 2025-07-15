using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Feedbacks;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Statistics
{
    public class AllFeedbackDTO
    {
        public DoctorDTO Doctor { get; set; }
        public PatientDTO Patient { get; set; }
        public FeedbackDTO Feedback { get; set; }
        public TreatmentServiceDTO TreatmentService { get; set; }

    }

}
