using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {

        private readonly FertilityCareDBContext _context;

        public AppointmentRepository(FertilityCareDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
        }

        public async Task<int> CountAppointmentByScheduleId(long scheduleId)
        {
            return await _context.Appointments.CountAsync(x => x.DoctorScheduleId == scheduleId);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Appointment>> FindAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> FindAllByStepIdAsync(long stepId)
        {
            return await _context.Appointments.Where(x => x.OrderStepId == stepId).ToListAsync();
        }

        public async Task<List<Appointment>> GetPageAsync(AppointmentQueryDTO query)
        {
            var baseQuery = _context.Appointments.AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.AppointmentDate))
            {
                baseQuery = baseQuery
                    .Where(x => x.AppointmentDate == DateOnly.Parse(query.AppointmentDate));
            }
            return await baseQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task<Appointment> FindByIdAsync(Guid id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with ID {id} not found.");
            }

            return appointment;
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if(appointment is null)
            {
                return false;
            }

            return true;
        }

        public async Task<Appointment> SaveAsync(Appointment entity)
        {
            await _context.Appointments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Appointment> UpdateAsync(Appointment entity)
        {
            _context.Appointments.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> FindTop6RecentPatientsAsync(Guid doctorId)
        {
            var appointments = await _context.Appointments
                .Where(x => x.DoctorId == doctorId)
                .OrderByDescending(x => x.AppointmentDate)
                .ToListAsync(); 

            return appointments
                .DistinctBy(x => x.PatientId)
                .Take(6);
        }


        public async Task<IEnumerable<Appointment>> FindByDoctorIdAsync(Guid doctorId)
        {
           return await _context.Appointments.Where(x => x.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> FindAllByDoctorIdAsync(Guid doctorId)
        {
            return await _context.Appointments.Where(x => x.DoctorId == doctorId)
                .ToListAsync();
        }
    }
}
