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
    public class TreatmentStepRepository : ITreatmentStepRepository
    {

        private readonly FertilityCareDBContext _context;

        public TreatmentStepRepository(FertilityCareDBContext context)
        {
            _context = context;
        }

        public async Task DeleteByIdAsync(long id)
        {
            var loadedStep = await _context.TreatmentSteps.FindAsync(id);
            if (loadedStep == null)
                throw new NotFoundException($"TreatmentStep with ID {id} not found!");

            _context.TreatmentSteps.Remove(loadedStep);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TreatmentStep>> FindAllAsync()
        {
           return await _context.TreatmentSteps.ToListAsync();
        }

        public async Task<TreatmentStep> FindByIdAsync(long id)
        {
            var loadedStep = await _context.TreatmentSteps.FindAsync(id);
            if (loadedStep == null)
                throw new NotFoundException($"TreatmentStep with ID {id} not found!");
            return loadedStep;
            
        }

        public async Task<bool> IsExistAsync(long id)
        {
            var loadedStep = await _context.TreatmentSteps.FindAsync(id);
            if (loadedStep == null)
                return false;
            return true;
        }

        public async Task<TreatmentStep> SaveAsync(TreatmentStep entity)
        {
            _context.TreatmentSteps.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TreatmentStep> UpdateAsync(TreatmentStep entity)
        {
            _context.TreatmentSteps.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
