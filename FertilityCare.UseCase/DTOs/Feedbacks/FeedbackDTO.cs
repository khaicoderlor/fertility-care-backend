﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.TreatmentServices;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class FeedbackDTO
    {
        public string Id { get; set; }
        public PatientDTO Patient { get; set; }
        public DoctorDTO? Doctor { get; set; }
        public TreatmentServiceDTO? TreatmentService { get; set; }
        public bool Status { get; set; }
        public decimal Rating { get; set; }
        public string? Comment { get; set; }
        public string CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
    }
}
