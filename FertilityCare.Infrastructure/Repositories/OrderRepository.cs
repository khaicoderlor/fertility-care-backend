using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
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
    public class OrderRepository : IOrderRepository
    {

        private readonly FertilityCareDBContext _context;

        public OrderRepository(FertilityCareDBContext context)
        {
            _context = context;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> FindByIdAsync(Guid id)
        {
            var result = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                throw new NotFoundException($"Order with ID {id} not found.");
            }

            return result;
        }

        public async Task<IEnumerable<Order>> FindAllByDoctorIdAsync(Guid doctorId)
        {
            return await _context.Orders.Where(x => x.DoctorId == doctorId).ToListAsync();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var orderExists = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (orderExists is null)
            {
                return false;
            }

            return true;
        }

        public async Task<Order> SaveAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Order>> FindAllByPatientIdAsync(Guid patientId)
        {
            return await _context.Orders.Where(x => x.PatientId == patientId).ToListAsync();
        }

        public async Task<int> CountDistinctActivePatientsAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == OrderStatus.InProgress)
                .Select(o => o.PatientId)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> CountDistinctDoctorsAsync()
        {
            return await _context.Orders
            .Where(o => o.Status == OrderStatus.InProgress)
            .Select(o => o.DoctorId)
            .Distinct()
            .CountAsync();
        }
        public async Task<int> CountAllOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }
    }
}
