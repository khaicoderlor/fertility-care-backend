using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
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

namespace FertilityCare.UseCase.DTOs.Appointments
{
    public class AppointmentDTO
    {
        public string? Id { get; set; }

        public PatientDTO? Patient { get; set; }

        public DoctorDTO? Doctor { get; set; }

        public DoctorScheduleDTO? DoctorSchedule { get; set; }

        public TreatmentServiceDTO? TreatmentService { get; set; }

        public OrderStepDTO? OrderStep { get; set; }

        public string? AppointmentDate { get; set; }

        public string? StartTime { get; set; }

        public string? EndTime { get; set; }

        public string? Status { get; set; }

        public string? CancellationReason { get; set; }

        public string? Note { get; set; }

        public decimal? ExtraFee { get; set; }

        public string? PaymentStatus { get; set; }

        public string? CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }

    }
}
