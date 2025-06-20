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
    public class EmbryoGainedRepository : IEmbryoGainedRepository
    {
        private readonly FertilityCareDBContext _context;
        public EmbryoGainedRepository(FertilityCareDBContext context)
        {
            _context = context;
        }
        public async Task DeleteByIdAsync(long id)
        {
            var loadedEmbryo = await _context.EmbryoGaineds.FindAsync(id);
            if (loadedEmbryo is null)
            {
                throw new NotFoundException($"EmbryoGained with ID {id} not found!");
            }
            _context.EmbryoGaineds.Remove(loadedEmbryo);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmbryoGained>> FindAllAsync()
        {
            return await _context.EmbryoGaineds.ToListAsync();
        }

        public async Task<EmbryoGained> FindByIdAsync(long id)
        {
            var loadedEmbryo = await _context.EmbryoGaineds.FindAsync(id);
            if (loadedEmbryo is null)
            {
                throw new NotFoundException($"EmbryoGained with ID {id} not found!");
            }
            return loadedEmbryo;
        }

        public async Task<bool> IsExistAsync(long id)
        {
            var loadedEmbryo = await _context.EmbryoGaineds.FindAsync(id);
            if (loadedEmbryo is null)
            {
                return false;
            }
            return true;
        }

        public async Task<EmbryoGained> SaveAsync(EmbryoGained entity)
        {
            await _context.EmbryoGaineds.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<EmbryoGained> UpdateAsync(EmbryoGained entity)
        {
            _context.EmbryoGaineds.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
