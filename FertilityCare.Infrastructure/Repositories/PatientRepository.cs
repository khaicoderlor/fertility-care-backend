using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FertilityCare.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly FertilityCareDBContext _context;
        public PatientRepository(FertilityCareDBContext context)
        {
            _context = context;
        }
        public async Task DeleteByIdAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                throw new NotFoundException($"Patient with ID {id} not found!");
            }
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Patient>> FindAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient> FindByIdAsync(Guid id)
        {
            var loadedPatient = await _context.Patients.FindAsync(id);
            if (loadedPatient is null)
            {
                throw new NotFoundException($"Patient with ID {id} not found!");
            }
            return loadedPatient;
        }

        public async Task<Patient> FindByProfileIdAsync(Guid profileId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.UserProfileId == profileId);
            if(patient is null)
            {
                throw new NotFoundException($"Patient with profile ID {profileId.ToString()} not found!");
            }

            return patient;
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var loadedPatient =  await _context.Patients.FindAsync(id);
            if (loadedPatient is null)
            {
                return false;
            }
            return true;
        }

        public async Task<Patient> SaveAsync(Patient entity)
        {
            await _context.Patients.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Patient> UpdateAsync(Patient entity)
        {
            _context.Patients.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
