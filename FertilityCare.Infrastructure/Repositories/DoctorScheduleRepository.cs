using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Repositories
{
    public class DoctorScheduleRepository : IDoctorScheduleRepository
    {
        private readonly FertilityCareDBContext _context;

        public DoctorScheduleRepository(FertilityCareDBContext context)
        {
            _context = context;
        }

        public async Task<DoctorSchedule> SaveAsync(DoctorSchedule entity)
        {
            await _context.DoctorSchedules.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<DoctorSchedule> UpdateAsync(DoctorSchedule entity)
        {
            var existing = await _context.DoctorSchedules.FindAsync(entity.Id);
            if (existing == null)
                throw new NotFoundException("Doctor schedule not found");

            _context.Entry(existing).CurrentValues.SetValues(entity);
            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteByIdAsync(long id)
        {
            var schedule = await _context.DoctorSchedules.FindAsync(id);
            if (schedule == null)
                throw new NotFoundException("Doctor schedule not found");

            _context.DoctorSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DoctorSchedule>> FindAllAsync()
        {
            return await _context.DoctorSchedules.ToListAsync();
        }

        public async Task<DoctorSchedule> FindByIdAsync(long id)
        {
            var schedule = await _context.DoctorSchedules
                .FirstOrDefaultAsync(ds => ds.Id == id);

            if (schedule == null)
                throw new NotFoundException("Doctor schedule not found");

            return schedule;
        }

        public async Task<bool> IsExistAsync(long id)
        {
            var result = await _context.DoctorSchedules.FirstOrDefaultAsync(ds => ds.Id == id);
            if (result == null)
            {
                return false;
            }

            return true;
        }

        public Task<IQueryable<DoctorSchedule>> FindAllQueryableAsync()
        {
            return Task.FromResult(
                _context.DoctorSchedules
                        .Include(ds => ds.Doctor)
                        .Include(ds => ds.Slot)
                        .AsQueryable()
            );
        }

        public async Task BulkInsertAsync(IEnumerable<DoctorSchedule> schedules)
        {
            await _context.DoctorSchedules.AddRangeAsync(schedules);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DoctorSchedule>> GetSchedulesByDateAndDoctorAsync(string workDate, string doctorId)
        {
            var parsedDate = DateOnly.Parse(workDate);
            var parsedDoctorId = Guid.Parse(doctorId);

            // Lấy tất cả các schedules hợp lệ
            var schedules = await _context.DoctorSchedules
                .Include(ds => ds.Doctor)
                .Include(ds => ds.Slot)
                .Where(ds =>
                    ds.WorkDate == parsedDate &&
                    ds.DoctorId == parsedDoctorId &&
                    ds.IsAcceptingPatients)
                .ToListAsync();

            // Lọc lại theo số lượng appointments thực tế
            var validSchedules = new List<DoctorSchedule>();

            foreach (var schedule in schedules)
            {
                var appointmentCount = await _context.Appointments
                    .CountAsync(a => a.DoctorScheduleId == schedule.Id);

                if (!schedule.MaxAppointments.HasValue || appointmentCount < schedule.MaxAppointments.Value)
                {
                    validSchedules.Add(schedule);
                }
            }

            return validSchedules;
        }

    }
}
