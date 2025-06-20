using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.UseCase.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FertilityCare.Infrastructure.Repositories
{
    public class PrescriptionItemRepository : IPrescriptionItemRepository
    {
        private readonly FertilityCareDBContext _context;
        public PrescriptionItemRepository(FertilityCareDBContext context)
        {
            _context = context;
        }
        public async Task DeleteByIdAsync(long id)
        {
            var loadedItem = await _context.PrescriptionItems.FindAsync(id);
            if (loadedItem is null)
                throw new KeyNotFoundException($"PrescriptionItem with ID {id} not found!");
            _context.PrescriptionItems.Remove(loadedItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PrescriptionItem>> FindAllAsync()
        {
            return await _context.PrescriptionItems.ToListAsync();
        }

        public async Task<PrescriptionItem> FindByIdAsync(long id)
        {
            var loadedItem = await _context.PrescriptionItems.FindAsync(id);
            if(loadedItem is null)
                throw new KeyNotFoundException($"PrescriptionItem with ID {id} not found!");
            return loadedItem;
        }

        public async Task<bool> IsExistAsync(long id)
        {
            var loadedItem = await _context.PrescriptionItems.FindAsync(id);
            if (loadedItem is null)
                return false;
            return true;
        }

        public async Task<PrescriptionItem> SaveAsync(PrescriptionItem entity)
        {
            await _context.PrescriptionItems.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<PrescriptionItem> UpdateAsync(PrescriptionItem entity)
        {
            _context.PrescriptionItems.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
