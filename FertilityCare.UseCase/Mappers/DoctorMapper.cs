using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.DoctorSchedules;
using FertilityCare.UseCase.DTOs.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Mappers
{
    public static class DoctorMapper
    {

        public static DoctorDTO MapToDoctorDTO(this Doctor doctor)
        {
            return new DoctorDTO
            {
                Id = doctor.Id.ToString(),
                Profile = doctor.UserProfile.MapToProfileDTO(),
                Degree = doctor.Degree,
                Specialization = doctor.Specialization,
                YearsOfExperience = doctor.YearsOfExperience,
                Biography = doctor.Biography,
                Rating = doctor.Rating,
                PatientsServed = doctor.PatientsServed,
                UpdatedAt = doctor.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss"),
                DoctorSchedules = doctor.DoctorSchedules.Select(x => x.MapToScheduleDTO()).ToList()
            }; 
        }
    }
}
