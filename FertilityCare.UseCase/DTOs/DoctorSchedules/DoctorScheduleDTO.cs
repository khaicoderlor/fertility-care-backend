using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.DoctorSchedules
{
    public class DoctorScheduleDTO
    {
        public long Id { get; set; }

        public string? DoctorId { get; set; }

        public string? WorkDate { get; set; }

        public SlotDTO? Slot { get; set; }

        public int? MaxAppointments { get; set; }

        public bool IsAcceptingPatients { get; set; }

        public string? Note { get; set; }

        public string? CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }

    }
}
