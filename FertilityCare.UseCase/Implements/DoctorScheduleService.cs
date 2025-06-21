using Fertilitycare.Share.Comon;
using Fertilitycare.Share.Pagination;
using FertilityCare.Domain.Entities;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.DoctorSchedules;
using FertilityCare.UseCase.DTOs.Slots;
using FertilityCare.UseCase.DTOs.UserProfiles;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.UseCase.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Implements
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IDoctorScheduleRepository _scheduleRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISlotRepository _slotRepository;

        public DoctorScheduleService(
            IDoctorScheduleRepository scheduleRepository,
            IDoctorRepository doctorRepository,
            ISlotRepository slotRepository)
        {
            _scheduleRepository = scheduleRepository;
            _doctorRepository = doctorRepository;
            _slotRepository = slotRepository;
        }

        public async Task CreateRecurringScheduleAsync(CreateRecurringScheduleRequestDTO request)
        {
            var doctor = await _doctorRepository.FindByIdAsync(request.DoctorId);
            if (doctor == null)
                throw new NotFoundException("Doctor not found");

            var schedules = new List<DoctorSchedule>();

            for (var date = request.StartDate.Value; date <= request.EndDate.Value; date = date.AddDays(1))
            {
                if (!request.WorkingDays.Contains(date.DayOfWeek))
                    continue;

                foreach (var slotId in request.SlotIds)
                {
                    var schedule = new DoctorSchedule
                    {
                        DoctorId = request.DoctorId,
                        WorkDate = date,
                        SlotId = slotId,
                        MaxAppointments = request.MaxAppointments,
                        Note = request.Note,
                        CreatedAt = DateTime.Now,
                        IsAcceptingPatients = true
                    };

                    schedules.Add(schedule);
                }
            }
            await _scheduleRepository.BulkInsertAsync(schedules);
        }

        public async Task<DoctorScheduleDTO> CreateScheduleAsync(CreateDoctorScheduleRequestDTO request)
        {
            // Kiểm tra Doctor có tồn tại không
            var doctor = await _doctorRepository.FindByIdAsync(request.DoctorId);
            if (doctor == null)
                throw new NotFoundException("Doctor not found");

            // Convert DateTime sang TimeOnly và DateOnly
            var startTime = TimeOnly.FromDateTime(request.StartTime);
            var endTime = TimeOnly.FromDateTime(request.EndTime);
            var workDate = DateOnly.FromDateTime(request.StartTime);

            // Tìm slot theo khung giờ đã có trong DB
            var slot = await _slotRepository.FindSlotAsync(startTime, endTime);
            if (slot == null)
                throw new NotFoundException("Slot not found for the provided time range");

            // Tạo DoctorSchedule
            var newSchedule = new DoctorSchedule
            {
                DoctorId = request.DoctorId,
                WorkDate = workDate,
                SlotId = slot.Id,
                MaxAppointments = request.MaxAppointments,
                IsAcceptingPatients = request.IsAcceptingPatients,
                Note = request.Note,
                CreatedAt = DateTime.Now,
            };

            var savedSchedule = await _scheduleRepository.SaveAsync(newSchedule);
            return savedSchedule.MapToScheduleDTO();
        }

        public async Task<bool> DeleteScheduleAsync(long scheduleId)
        {
            await _scheduleRepository.DeleteByIdAsync(scheduleId);
            return true;
        }

        public async Task<IEnumerable<DoctorScheduleDTO>> FindAllSchedulesAsync()
        {
            var all = await _scheduleRepository.FindAllAsync();
            return all.Select(s => s.MapToScheduleDTO());
        }
    }
}
