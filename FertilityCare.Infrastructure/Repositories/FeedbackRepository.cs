using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Feedbacks;
using FertilityCare.UseCase.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FertilityCare.Infrastructure.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FertilityCareDBContext _context;
        public FeedbackRepository(FertilityCareDBContext context)
        {
            _context = context;
        }
        public async Task DeleteByIdAsync(Guid id)
        {
            var loadedFeedback = await _context.Feedbacks.FindAsync(id);
            if (loadedFeedback is null)
            {
                throw new NotFoundException($"Feedback id {id} not exist!");
            }
            _context.Feedbacks.Remove(loadedFeedback);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Feedback>> FindAllAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<Feedback> FindByIdAsync(Guid id)
        {
            var loadedFeedback = await _context.Feedbacks.FindAsync(id);
            if (loadedFeedback is null)
            {
                throw new NotFoundException($"Feedback id {id} not exist!");
            }
            return loadedFeedback;
        }

        public async Task<List<Feedback>> FindDoctorByIdAsync(Guid doctorId)
        {
            return await _context.Feedbacks
                .Where(f => f.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<List<Feedback>> FindTreatmentServiceByIdAsync(Guid treatmentId)
        {
            return await _context.Feedbacks
                .Where(f => f.TreatmentServiceId == treatmentId)
                .ToListAsync();
        }

        public async Task<List<Feedback>> GetAllFeedbacksAsync(int pageNumber, int pageSize)
        {
            return await _context.Feedbacks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<List<Feedback>> GetFeedbackByAllIdAsync(FeedbackQueryDTO query, int pageNumber, int pageSize)
        {
            var loadedFeedbacks = _context.Feedbacks.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.DoctorId))
            {
                loadedFeedbacks = loadedFeedbacks.Where(f => f.DoctorId.ToString() == query.DoctorId);
            }
            if(!string.IsNullOrWhiteSpace(query.PatientId))
            {
                loadedFeedbacks = loadedFeedbacks.Where(f => f.PatientId.ToString() == query.PatientId);
            }
            if(!string.IsNullOrWhiteSpace(query.TreatmentServiceId))
            {
                loadedFeedbacks = loadedFeedbacks.Where(f => f.TreatmentServiceId.ToString() == query.TreatmentServiceId);
            }
            if(!string.IsNullOrWhiteSpace(query.rating.ToString()))
            {
                loadedFeedbacks = loadedFeedbacks.Where(f => f.Rating <= query.rating);
            }
            //loadedFeedbacks.Where(f => f.Status == true);
            return loadedFeedbacks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var loadedFeedback = await _context.Feedbacks.FindAsync(id);
            if (loadedFeedback is null)
            {
                return false;
            }
            return true;
        }

        public async Task<Feedback> SaveAsync(Feedback entity)
        {
            await _context.Feedbacks.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Feedback> UpdateAsync(Feedback entity)
        {
            _context.Feedbacks.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
