using Fertilitycare.Share.Comon;
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
                if (!request.WorkingDays.Contains(date.DayOfWeek) || date.DayOfWeek == DayOfWeek.Sunday)
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

        public async Task<IEnumerable<DoctorScheduleDTO>> GetAllSchedulesAsync(Guid doctorId)
        {
            var all = await _scheduleRepository.FindAllAsync();
            return all.Where(s => s.DoctorId == doctorId)
                      .Select(s => s.MapToScheduleDTO());
        }

        public async Task<DoctorScheduleDTO?> GetScheduleByIdAsync(long scheduleId)
        {
            var result = await _scheduleRepository.FindByIdAsync(scheduleId);
            return result?.MapToScheduleDTO();
        }

        public async Task<IEnumerable<DoctorScheduleDTO>> GetSchedulesPagedAsync(PaginationRequestDTO request)
        {
            var query = await _scheduleRepository.FindAllQueryableAsync();


            return query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => x.MapToScheduleDTO())
                .ToList();

        }

        public async Task<IEnumerable<SlotWithScheduleDTO>> GetSlotWithDoctorsByDateAsync(String workDate, string id)
        {
            var result = await _scheduleRepository.GetSchedulesByDateAndDoctorAsync(workDate, id);
            return result.MapToSlotWithScheduleIdsDTO();
        }

        public async Task<IEnumerable<DoctorScheduleViewDTO>> GetWeeklySchedulesAsync(Guid doctorId, DateOnly weekDate)
        {
            var startOfWeek = weekDate.AddDays(-(int)weekDate.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);

            var schedules = await _scheduleRepository.GetSchedulesByWeekAsync(doctorId, startOfWeek, endOfWeek);

            return schedules.Select(s => new DoctorScheduleViewDTO
            {
                WorkDate = s.WorkDate.ToString("dd/MM/yyyy"),
                StartTime = s.Slot.StartTime.ToString("HH:mm"),
                EndTime = s.Slot.EndTime.ToString("HH:mm"),
                FirstName = s.Doctor.UserProfile.FirstName!,
                MiddleName = s.Doctor.UserProfile.MiddleName,
                LastName = s.Doctor.UserProfile.LastName!
            }).ToList();
        }

        public async Task<DoctorScheduleDTO> UpdateScheduleAsync(UpdateDoctorScheduleRequestDTO request)
        {
            var schedule = await _scheduleRepository.FindByIdAsync(request.Id);
            if (schedule == null)
                throw new NotFoundException("Schedule not found");

            var startTime = TimeOnly.FromDateTime(request.StartTime);
            var endTime = TimeOnly.FromDateTime(request.EndTime);
            var workDate = DateOnly.FromDateTime(request.StartTime);

            // Tìm slot có sẵn
            var slot = await _slotRepository.FindSlotAsync(startTime, endTime);
            if (slot == null)
                throw new NotFoundException("Slot not found for the provided time range");

            // Cập nhật thông tin
            schedule.WorkDate = workDate;
            schedule.SlotId = slot.Id;
            schedule.MaxAppointments = request.MaxAppointments;
            schedule.IsAcceptingPatients = request.IsAcceptingPatients;
            schedule.Note = request.Note;
            schedule.UpdatedAt = DateTime.Now;

            var updated = await _scheduleRepository.UpdateAsync(schedule);
            return updated.MapToScheduleDTO();
        }
    }
}
