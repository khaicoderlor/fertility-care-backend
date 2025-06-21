using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.DoctorSchedules;
using FertilityCare.UseCase.DTOs.OrderSteps;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Mappers
{
    public static class AppointmentMapper
    {

        public static AppointmentDTO MapToAppointmentDTO(this Appointment appointment)
        {
            return new AppointmentDTO
            {
                Id = appointment.Id.ToString(),
                Patient = appointment.Patient?.MapToPatientDTO(),
                Doctor = appointment.Doctor?.MapToDoctorDTO(),
                DoctorSchedule = appointment.DoctorSchedule?.MapToScheduleDTO(),
                TreatmentService = appointment.TreatmentService?.MapToTreatmentServiceDTO(),
                OrderStep = appointment.OrderStep?.MapToStepDTO(),
                AppointmentDate = appointment.AppointmentDate.ToString("dd/MM/yyyy"),       
                StartTime = appointment.StartTime.ToString("HH:mm"),
                EndTime = appointment.EndTime.ToString("HH:mm"),
                Status = appointment.Status.ToString(),
                CancellationReason = appointment.CancellationReason,
                Note = appointment.Note,
                ExtraFee = appointment.ExtraFee,
                PaymentStatus = appointment.PaymentStatus.ToString(),
                CreatedAt = appointment.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                UpdatedAt = appointment.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }

        public static AppointmentFollowStep MapToAppointmentFollowStep(this Appointment appointment)
        {
            var profilePatient = appointment.Patient?.UserProfile;
            var profileDoctor = appointment.Doctor?.UserProfile;

            return new AppointmentFollowStep
            {
                Id = appointment.Id.ToString(),
                PatientName = $"{profilePatient.FirstName} {profilePatient.MiddleName} {profilePatient.LastName}" ?? string.Empty,
                AppointmentDate = appointment.AppointmentDate.ToString("dd/MM/yyyy"),
                Slot = appointment.DoctorSchedule?.Slot?.SlotNumber ?? -1,
                StartTime = appointment.StartTime.ToString("HH:mm"),
                EndTime = appointment.EndTime.ToString("HH:mm"),
                Type = appointment.Type.ToString(),
                DoctorName = $"{profileDoctor.FirstName} {profileDoctor.MiddleName} {profileDoctor.LastName}" ?? string.Empty,
                Status = appointment.Status.ToString(),
                DoctorId = appointment.DoctorId.ToString(),
                PaymentStatus = appointment.PaymentStatus.ToString(),
                ExtraFee = appointment.ExtraFee ?? decimal.Zero,
                Note = appointment.Note ?? string.Empty
            };
        }

    }
}
