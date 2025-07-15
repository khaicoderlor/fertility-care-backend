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
        public static DoctorScheduleDTO MapToDoctorScheduleDTO(this DoctorSchedule schedule)
        {
            return new DoctorScheduleDTO
            {
                Id = schedule.Id,
                DoctorId = schedule.DoctorId.ToString(),
                WorkDate = schedule.WorkDate.ToString("yyyy-MM-dd"),
                Slot = schedule.Slot?.MapToSlotDTO(),
                MaxAppointments = schedule.MaxAppointments,
                IsAcceptingPatients = schedule.IsAcceptingPatients,
                Note = schedule.Note,
                CreatedAt = schedule.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdatedAt = schedule.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}
