using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Appointments
{
    public class AppointmentFollowStep
    {
        public string Id { get; set; } = string.Empty;

        public string PatientName { get; set; } = string.Empty;

        public string AppointmentDate { get; set; } = string.Empty;

        public int Slot { get; set; }

        public string StartTime { get;set; } = string.Empty;

        public string EndTime { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;   
            
        public string DoctorName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string DoctorId { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;

        public decimal ExtraFee { get; set; } = decimal.Zero;

        public string Note { get; set; } = string.Empty;    
    
    }
}
