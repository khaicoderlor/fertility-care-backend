using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class BestRateDoctor
    {
        public DoctorDTO Doctor { get; set; }

        public decimal Rating { get; set; }

        public int TotalFeedbacks { get; set; }
    }
}
