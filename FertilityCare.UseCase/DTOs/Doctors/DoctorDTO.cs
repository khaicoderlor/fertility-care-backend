using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.DoctorSchedules;
using FertilityCare.UseCase.DTOs.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Doctors
{
    public class DoctorDTO
    {
        public string? Id { get; set; }

        public ProfileDTO? Profile { get; set; }

        public string? Degree { get; set; }

        public string? Specialization { get; set; }

        public int? YearsOfExperience { get; set; }

        public string? Biography { get; set; }

        public decimal? Rating { get; set; }

        public int? PatientsServed { get; set; }

        public string? CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }

        public List<DoctorScheduleDTO> DoctorSchedules { get; set; }
    }
}