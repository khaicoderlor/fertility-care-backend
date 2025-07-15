using FertilityCare.UseCase.DTOs.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class SecondFeedbackLatest
    {
        public string Content { get; set; }

        public decimal Rating { get; set; } 

        public string CreatedAt { get; set; }

        public PatientDTO Patient { get; set; }

    }
}
