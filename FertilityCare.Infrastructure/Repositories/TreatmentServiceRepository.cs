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
    public class TreatmentServiceRepository : ITreatmentServiceRepository
    {
        private readonly FertilityCareDBContext _context;

        public TreatmentServiceRepository(FertilityCareDBContext context)
        {
            _context = context;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var service = await _context.TreatmentServices.FindAsync(id);
            if (service == null)
                throw new NotFoundException($"TreatmentService with ID {id} not found!");

            _context.TreatmentServices.Remove(service);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TreatmentService>> FindAllAsync()
        {
            return await _context.TreatmentServices.ToListAsync();
        }

        public async Task<TreatmentService> FindByIdAsync(Guid id)
        {
            var loadedTreament = await _context.TreatmentServices.FindAsync(id);
            if(loadedTreament is null)
            {
                throw new NotFoundException($"TreatmentService with ID {id} not found!");
            }
            return loadedTreament;
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var loadedTreament = await _context.TreatmentServices.FindAsync(id);
            if (loadedTreament is null)
            {
                return false;
            }
            return true;
        }

        public async Task<TreatmentService> SaveAsync(TreatmentService entity)
        {
            await _context.TreatmentServices.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TreatmentService> UpdateAsync(TreatmentService entity)
        {
            _context.TreatmentServices.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
