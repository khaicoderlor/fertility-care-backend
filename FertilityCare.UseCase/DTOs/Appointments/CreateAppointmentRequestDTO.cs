using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Appointments
{
    public class CreateAppointmentRequestDTO
    {
        public string? PatientId { get; set; }

        public string? DoctorId { get; set; }

        public long DoctorScheduleId { get; set; }

        public string? TreatmentServiceId { get; set; }

        public long OrderStepId { get; set; }

        public string? BookingEmail { get; set; }

        public string? BookingPhone { get; set; }

        public string? Note { get; set; }   

    }
}