using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class PatientDashboard
    {
        public string? PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? TreatmentName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? OrderId { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Status { get; set; }
        public long? TotalEggs { get; set; }
        public int? TotalEmbryos { get; set; }
        public bool? IsFrozen { get; set; }
    }
}
