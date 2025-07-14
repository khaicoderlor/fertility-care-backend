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
    public class EmbryoTransferRepository : IEmbryoTransferRepository
    {
        private readonly FertilityCareDBContext _context;
        public EmbryoTransferRepository(FertilityCareDBContext context)
        {
            _context = context;
        }
        public async Task DeleteByIdAsync(long id)
        {
            var entity = await _context.EmbryoTransfers.FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"EmbryoTransfer with ID {id} not found.");
            }
            _context.EmbryoTransfers.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<int> CountTotalEmbryoTransfersAsync()
        {
            return await _context.EmbryoTransfers.CountAsync();
        }


        public async Task<IEnumerable<EmbryoTransfer>> FindAllAsync()
        {
            return await _context.EmbryoTransfers.ToListAsync();
        }

        public async Task<IEnumerable<EmbryoTransfer>> FindAllByOrderIdAsync(Guid guid)
        {
            return await _context.EmbryoTransfers.Where(x => x.OrderId == guid).ToListAsync();
        }

        public async Task<EmbryoTransfer> FindByIdAsync(long id)
        {
            var entity = await _context.EmbryoTransfers.FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"EmbryoTransfer with ID {id} not found.");
            }
            return entity;
        }
        public async Task<bool> IsExistAsync(long id)
        {
            var entity = await _context.EmbryoTransfers.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            return true;
        }

        public async Task<EmbryoTransfer> SaveAsync(EmbryoTransfer entity)
        {
            await _context.EmbryoTransfers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<EmbryoTransfer> UpdateAsync(EmbryoTransfer entity)
        {
            _context.EmbryoTransfers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
