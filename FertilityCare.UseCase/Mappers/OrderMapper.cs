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
                Note = order.Note,
                TotalAmount = order.TotalAmount,
                TotalEgg = order.TotalEgg,
                StartDate = order.StartDate.ToString("dd/MM/yyyy"),
                EndDate = order.EndDate?.ToString("dd/MM/yyyy"),
                CreatedAt = order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                UpdatedAt = order.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss"),
                OrderSteps = order.OrderSteps?.Select(x => x.MapToStepDTO()).ToList()
            };
        }

    }
}
