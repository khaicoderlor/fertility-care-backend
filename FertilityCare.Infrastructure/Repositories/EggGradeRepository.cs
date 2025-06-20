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
    public class EggGradeRepository : IEggGradeRepository
    {
        private readonly FertilityCareDBContext _context;
        public EggGradeRepository(FertilityCareDBContext context)
        {
            _context = context;
        }
        public async Task DeleteByIdAsync(long id)
        {
            var loadedEgg = await _context.EggGaineds.FindAsync(id);
            if (loadedEgg == null)
            {
                throw new NotFoundException($"EggGained with ID {id} not found!");
            }
            _context.EggGaineds.Remove(loadedEgg);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EggGained>> FindAllAsync()
        {
            return await _context.EggGaineds.ToListAsync();
        }

        public async Task<EggGained> FindByIdAsync(long id)
        {
            var loadedEgg = await _context.EggGaineds.FindAsync(id);
            if(loadedEgg is null)
            {
                throw new NotFoundException($"EggGained with ID {id} not found!");
            }
            return loadedEgg;
        }

        public async Task<bool> IsExistAsync(long id)
        {
            var loadedEgg = await _context.EggGaineds.FindAsync(id);
            if (loadedEgg is null)
            {
                return false;
            }
            return true;
        }

        public async Task<EggGained> SaveAsync(EggGained entity)
        {
            await _context.EggGaineds.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<EggGained> UpdateAsync(EggGained entity)
        {
            _context.EggGaineds.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
