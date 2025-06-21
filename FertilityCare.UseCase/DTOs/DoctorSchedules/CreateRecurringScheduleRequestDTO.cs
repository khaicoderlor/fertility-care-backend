using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.DoctorSchedules
{
    public class CreateRecurringScheduleRequestDTO
    {
        public Guid DoctorId { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public List<DayOfWeek>? WorkingDays { get; set; }
        public List<long>? SlotIds { get; set; }

        public int? MaxAppointments { get; set; }
        public string? Note { get; set; }
    }


}
