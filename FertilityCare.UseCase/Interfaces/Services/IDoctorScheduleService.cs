using Fertilitycare.Share.Comon;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.DoctorSchedules;
using FertilityCare.UseCase.DTOs.Slots;
using FertilityCare.UseCase.DTOs.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IDoctorScheduleService
    {
        //CRUD
        Task<DoctorScheduleDTO> CreateScheduleAsync(CreateDoctorScheduleRequestDTO request);

        Task<DoctorScheduleDTO> UpdateScheduleAsync(UpdateDoctorScheduleRequestDTO request);

        Task<bool> DeleteScheduleAsync(long scheduleId);

        Task<IEnumerable<DoctorScheduleDTO>> FindAllSchedulesAsync();

        Task<IEnumerable<DoctorScheduleDTO>> GetAllSchedulesAsync(Guid doctorId);

        Task<DoctorScheduleDTO?> GetScheduleByIdAsync(long scheduleId);

        Task<IEnumerable<DoctorScheduleDTO>> GetSchedulesPagedAsync(PaginationRequestDTO request);

        Task<IEnumerable<SlotWithScheduleDTO>> GetSlotWithDoctorsByDateAsync(string workDate, string id);

        Task CreateRecurringScheduleAsync(CreateRecurringScheduleRequestDTO request);

        Task<IEnumerable<DoctorScheduleViewDTO>> GetWeeklySchedulesAsync(Guid doctorId, DateOnly weekDate);
    }
}
