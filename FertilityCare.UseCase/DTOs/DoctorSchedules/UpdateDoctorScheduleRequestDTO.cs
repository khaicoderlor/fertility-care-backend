using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.DoctorSchedules
{
    public class UpdateDoctorScheduleRequestDTO
    {
        public long Id { get; set; }

        public DateTime StartTime { get; set; }  

        public DateTime EndTime { get; set; }

        public int? MaxAppointments { get; set; }

        public bool IsAcceptingPatients { get; set; }

        public string? Note { get; set; }
    }
}
