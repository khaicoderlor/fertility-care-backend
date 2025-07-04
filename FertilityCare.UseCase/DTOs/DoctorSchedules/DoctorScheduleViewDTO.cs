using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.DoctorSchedules
{
    public class DoctorScheduleViewDTO
    {
        public string WorkDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        public string ShiftType { get; set; }

        public string DoctorId { get; set; }

        public long ScheduleId { get; set; }

        public bool IsWorkingDay { get; set; }

    }
}
