using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Orders
{
    public class CreateOrderRequestDTO
    {
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? MedicalHistory { get; set; }

        public string? PartnerFullName { get; set; }

        public string? PartnerEmail { get; set; }

        public string? PartnerPhone { get; set; }

        public string? UserProfileId { get; set; }

        public string? DoctorId { get; set; }

        public long DoctorScheduleId { get; set; }

        public string TreatmentServiceId { get; set; }

        public string? Note { get; set; }

        public string? BookingEmail { get; set; }

        public string? BookingPhone { get; set; }

    }
}
