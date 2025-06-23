using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Orders;
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
    public static class OrderMapper
    {

        public static OrderDTO MapToOderDTO(this Order order)
        {

            return new OrderDTO
            {
                Id = order.Id.ToString(),
                Patient = order.Patient.MapToPatientDTO(),
                TreatmentService = order.TreatmentService.MapToTreatmentServiceDTO(),
                Doctor = order.Doctor.MapToDoctorDTO(),
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                TotalEgg = order.TotalEgg,
                StartDate = order.StartDate.ToString("dd/MM/yyyy"),
                EndDate = order.EndDate?.ToString("dd/MM/yyyy"),
                UpdatedAt = order.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss"),
                OrderSteps = order.OrderSteps?.Select(x => x.MapToStepDTO()).ToList()
            };

        }

        public static OrderInfo MapToOrderInfo(this Order order)
        {
            var doctorProfile = order.Doctor.UserProfile;
            var patientProfile = order.Patient.UserProfile;
            return new OrderInfo
            {
                Id = order.Id.ToString(),
                TreatmentServiceName = order.TreatmentService.Name,
                DoctorName = $"{doctorProfile.FirstName} {doctorProfile.MiddleName} {doctorProfile.LastName}",
                PatientName = $"{patientProfile.FirstName} {patientProfile.MiddleName} {patientProfile.LastName}",
                StartDate = order.StartDate.ToString("dd/MM/yyyy"),
                EndDate = order.EndDate == null ? "#NoData" : order.EndDate?.ToString("dd/MM/yyyy"),
                Status = order.Status.ToString(),
                IsFrozen = order.IsFrozen,
                TotalEggs = order.TotalEgg,
                TotalAmount = order.TotalAmount,
            };
        }

    }
}