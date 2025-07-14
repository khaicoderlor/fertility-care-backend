using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.UseCase.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Repositories
{
    public class EggGainedRepository : IEggGainedRepository
    {
        private readonly FertilityCareDBContext _context;

        public EggGainedRepository(FertilityCareDBContext context)
        {
            _context = context;
        }

        public async Task BulkInsertAsync(IEnumerable<EggGained> eggs)
        {
            await _context.EggGaineds.AddRangeAsync(eggs);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EggGained>> FindAllByOrderIdAsync(Guid orderId)
        {
            return await _context.EggGaineds.Where(x => x.OrderId == orderId).ToListAsync();
        }

        public async Task<IEnumerable<EggGained>> GetUsableEggsByOrderIdAsync(Guid orderId)
        {
            return await _context.EggGaineds
            .Where(e => e.OrderId == orderId && e.IsUsable)
            .ToListAsync();
        }
        public async Task<int> CountEggGainedByMonthAsync(int month)
        {
            return await _context.EggGaineds
                .Where(e => e.DateGained.Month == month)
                .CountAsync();
        }

    }
}
