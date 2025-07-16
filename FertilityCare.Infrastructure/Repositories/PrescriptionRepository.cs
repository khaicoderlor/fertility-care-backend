using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.UseCase.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace FertilityCare.Infrastructure.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly FertilityCareDBContext _context;
        public PrescriptionRepository(FertilityCareDBContext context)
        {
            _context = context;
        }
        public async Task DeleteByIdAsync(Guid id)
        {
            var loadedPrescription = await _context.Prescriptions.FindAsync(id);
            if(loadedPrescription is null)
                throw new KeyNotFoundException($"Prescription with ID {id} not found!");
            _context.Prescriptions.Remove(loadedPrescription);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Prescription>> FindAllAsync()
        {
            return await _context.Prescriptions.ToListAsync();
        }

        public async Task<Prescription> FindByIdAsync(Guid id)
        {
            var loadedPrescription = await _context.Prescriptions.FindAsync(id);
            if(loadedPrescription is null)
                throw new KeyNotFoundException($"Prescription with ID {id} not found!");
            return loadedPrescription;
        }

        public async Task<IEnumerable<Prescription>> FindPrescriptionsByOrderIdAsync(Guid orderId)
        {
            return await _context.Prescriptions
                .Where(p => p.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> FindPrescriptionsByPatientIdAsync(Guid patientId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.PatientId.Equals(patientId));

            return await _context.Prescriptions
                .Where(p => p.OrderId.Equals(order.Id))
                .ToListAsync();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var loadedPrescription = await _context.Prescriptions.FindAsync(id);
            if (loadedPrescription is null)
                return false;
            return true;
        }

        public async Task<Prescription> SaveAsync(Prescription entity)
        {
            await _context.Prescriptions.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Prescription> UpdateAsync(Prescription entity)
        {
            _context.Prescriptions.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
