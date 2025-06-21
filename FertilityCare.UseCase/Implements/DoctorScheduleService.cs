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
        }
    }
}
