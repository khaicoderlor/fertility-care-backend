using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class PatientProgress
    {
        public PatientDTO Patient { get; set; }

        public DoctorDTO Doctor { get; set; }

        public OrderDTO Order { get; set; }

        public string ServiceName { get; set; }

        public int CurrentStep { get; set; }

        public int TotalSteps { get; set; }

        public string StartDate { get; set; }

        public string? EndDate { get; set; }

        public string Status { get; set; }
    }
}
