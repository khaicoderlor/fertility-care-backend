using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Orders;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class AllFeedbackDTO
    {
        public DoctorDTO Doctor { get; set; }
        public PatientDTO Patient { get; set; }
        public FeedbackDTO Feedback { get; set; }
        public TreatmentServiceDTO TreatmentService { get; set; }

    }
}
