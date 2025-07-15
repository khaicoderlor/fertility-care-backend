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
    public class OrderStepRepository : IOrderStepRepository
    {
        private readonly FertilityCareDBContext _context;

        public OrderStepRepository(FertilityCareDBContext context)
        {
            _context = context;
        }

        public async Task<int> CountOrderStepPlannedAsync()
        {
            return await _context.OrderSteps.CountAsync(os => os.Status == Domain.Enums.StepStatus.Planned);
        }

        public async Task DeleteByIdAsync(long id)
        {
            var orderStep = await _context.OrderSteps.FindAsync(id);
            if (orderStep != null)
            {
                _context.OrderSteps.Remove(orderStep);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderStep>> FindAllAsync()
        {
            return await _context.OrderSteps.ToListAsync();
        }

        public async Task<IEnumerable<OrderStep>> FindAllByOrderIdAsync(Guid orderId)
        {
            return await _context.OrderSteps.Where(os => os.OrderId == orderId).ToListAsync();
        }

        public async Task<OrderStep> FindByIdAsync(long id)
        {
            var result = await _context.OrderSteps.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                throw new NotFoundException($"OrderStep with ID {id} not found.");
            }

            return result;
        }

        public async Task<bool> IsExistAsync(long id)
        {
            var result =  await _context.OrderSteps.FirstOrDefaultAsync(os => os.Id == id);
            if (result is null)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<OrderStep>> SaveAllAsync(IEnumerable<OrderStep> orderSteps)
        {
            await _context.OrderSteps.AddRangeAsync(orderSteps);
            await _context.SaveChangesAsync();
            return orderSteps;
        }

        public async Task<OrderStep> SaveAsync(OrderStep entity)
        {
            _context.OrderSteps.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<OrderStep> UpdateAsync(OrderStep entity)
        {
            _context.OrderSteps.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}