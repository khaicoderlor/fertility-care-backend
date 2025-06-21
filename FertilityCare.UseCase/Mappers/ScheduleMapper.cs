using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.DoctorSchedules;
using FertilityCare.UseCase.DTOs.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Mappers
{
    public static class ScheduleMapper
    {

        public static DoctorScheduleDTO MapToScheduleDTO(this DoctorSchedule schedule)
        {
            return new DoctorScheduleDTO
            {
                Id = schedule.Id,
                DoctorId = schedule.DoctorId.ToString(),
                WorkDate = schedule.WorkDate.ToString("dd/MM/yyyy"),
                Slot = schedule.Slot?.MapToSlotDTO(),
                MaxAppointments = schedule.MaxAppointments,
                IsAcceptingPatients = schedule.IsAcceptingPatients,
                Note = schedule.Note,
                CreatedAt = schedule.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                UpdatedAt = schedule.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }

    }
}
