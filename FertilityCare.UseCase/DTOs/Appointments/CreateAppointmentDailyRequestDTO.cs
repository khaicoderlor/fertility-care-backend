using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Appointments
{
    public class CreateAppointmentDailyRequestDTO
    {
        public string PatientId { get; set; }

        public string DoctorId { get; set; }    
        
        public long DoctorScheduleId { get; set; }

        public long OrderStepId { get; set; }

        public string Type { get;set; }

        public decimal? Extrafee { get; set; } = 0;

        public string? Note { get; set; }

    }
}
