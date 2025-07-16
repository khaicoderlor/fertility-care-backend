using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.DoctorSchedules
{
    public class CreateDoctorScheduleRequestDTO
    {
        public string DoctorId { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; } 

        public TimeOnly EndTime { get; set; }

        public int? MaxAppointments { get; set; }

        public string? Note { get; set; }
    }
}
