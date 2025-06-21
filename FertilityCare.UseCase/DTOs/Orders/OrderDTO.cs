using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.OrderSteps;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Orders
{
    public class OrderDTO
    {
        public string? Id { get; set; }

        public PatientDTO? Patient { get; set; }

        public DoctorDTO? Doctor { get; set; }

        public TreatmentServiceDTO? TreatmentService { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }

        public string? Status { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Note { get; set; }

        public long? TotalEgg { get; set; }

        public string? CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }

        public List<OrderStepDTO>? OrderSteps { get; set; } 

        }
}
