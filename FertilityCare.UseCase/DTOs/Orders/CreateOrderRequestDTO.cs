using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FertilityCare.UseCase.DTOs.Orders
{
    public class CreateOrderRequestDTO
    {

        public string PatientId { get; set; } = string.Empty;

        public string DoctorId { get; set; } = string.Empty;

        public long DoctorScheduleId { get; set; } = 0;

        public string TreatmentServiceId { get; set; } = string.Empty;

    }
}
