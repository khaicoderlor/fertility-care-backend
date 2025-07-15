using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
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
    public class OrderStepPaymentRepository : IOrderStepPaymentRepository
    {
        private readonly FertilityCareDBContext _context;
        
        public OrderStepPaymentRepository(FertilityCareDBContext context)
        {
            _context = context;
        }

      

        public Task DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderStepPayment>> FindAllAsync()
        {
            return await _context.orderStepPayments.ToListAsync();
        }

        public async Task<IEnumerable<OrderStepPayment>> FindAllByPatientIdAsync(Guid patientId)
        {
            return await _context.orderStepPayments.Where(p => p.PatientId == patientId).ToListAsync();
        }

        public Task<OrderStepPayment> FindByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderStepPayment> SaveAsync(OrderStepPayment entity)
        {
            throw new NotImplementedException();
        }

        public Task<OrderStepPayment> UpdateAsync(OrderStepPayment entity)
        {
            throw new NotImplementedException();
        }
    }
}
