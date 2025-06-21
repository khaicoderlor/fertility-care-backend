using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Slots;
using FertilityCare.UseCase.DTOs.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Mappers
{
    public static class SlotMapper
    {

        public static SlotDTO MapToSlotDTO(this Slot slot)
        {
            return new SlotDTO
            {
                SlotNumber = slot.SlotNumber,
                StartTime = slot.StartTime.ToString("HH:mm"),
                EndTime = slot.EndTime.ToString("HH:mm"),
                CreatedAt = slot.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                UpdatedAt = slot.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss"),
            };
        }

        public static IEnumerable<SlotWithScheduleDTO> MapToSlotWithScheduleIdsDTO(this IEnumerable<DoctorSchedule> schedules)
        {
            return schedules.Select(x => x.MapToSlotScheduleDTO()).ToList();
        }

        public static SlotWithScheduleDTO MapToSlotScheduleDTO(this DoctorSchedule schedule)
        {
            return new SlotWithScheduleDTO
            {
                SlotId = schedule.SlotId,
                StartTime = schedule.Slot.StartTime.ToString("HH:mm"),
                EndTime = schedule.Slot.EndTime.ToString("HH:mm"),
                ScheduleId = schedule.Id,
            };
        }



    }
}
