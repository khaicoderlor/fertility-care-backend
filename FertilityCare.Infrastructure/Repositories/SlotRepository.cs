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
    public class SlotRepository : ISlotRepository
    {
        private readonly FertilityCareDBContext _context;

        public SlotRepository(FertilityCareDBContext context)
        {
            _context = context;
        }

        public Task DeleteByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Slot>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Slot> FindByIdAsync(long id)
        {
            var loadedSlot = await _context.Slots.FindAsync(id);
            if (loadedSlot is null)
            {
                throw new NotFoundException($"Slot with ID {id} not found.");
            }
            return loadedSlot;
        }

        public async Task<Slot?> FindSlotAsync(TimeOnly startTime, TimeOnly endTime)
        {
            return await _context.Slots
                .FirstOrDefaultAsync(s => s.StartTime == startTime && s.EndTime == endTime);
        }

        public async Task<bool> IsExistAsync(long id)
        {
            var slotExists = await _context.Slots.FindAsync(id);
            if (slotExists is null)
            {
                return false;
            }

            return true;
        }

        public Task<Slot> SaveAsync(Slot entity)
        {
            throw new NotImplementedException();
        }

        public Task<Slot> UpdateAsync(Slot entity)
        {
            throw new NotImplementedException();
        }
    }
}
