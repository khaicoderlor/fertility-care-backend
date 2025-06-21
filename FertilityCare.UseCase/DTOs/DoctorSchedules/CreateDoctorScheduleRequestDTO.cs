using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.DoctorSchedules
{
    public class CreateDoctorScheduleRequestDTO
    {
        public Guid DoctorId { get; set; }

        public DateTime StartTime { get; set; } 

        public DateTime EndTime { get; set; }

        public int? MaxAppointments { get; set; }

        public bool IsAcceptingPatients { get; set; } = true;

        public string? Note { get; set; }
    }
}
