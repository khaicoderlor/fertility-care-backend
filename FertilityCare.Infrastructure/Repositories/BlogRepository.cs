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
    public class BlogRepository : IBlogRepository
    {
        private readonly FertilityCareDBContext _context;
        public BlogRepository(FertilityCareDBContext context)
        {
            _context = context;
        }
        public async Task DeleteByIdAsync(Guid id)
        {
            var loadedBlog = await _context.Blogs.FindAsync(id);
            if (loadedBlog is null)
            {
                throw new NotFoundException($"Blog with id {id} not found.");
            }
            _context.Blogs.Remove(loadedBlog);
            await _context.SaveChangesAsync();
        }

        
        public async Task<IEnumerable<Blog>> FindAllAsync()
        {
            return await _context.Blogs.ToListAsync();
        }

        public async Task<Blog> FindByIdAsync(Guid id)
        {
            var loadedBlog = await _context.Blogs.FindAsync(id);
            if (loadedBlog is null)
            {
                throw new NotFoundException($"Blog with id {id} not found.");
            }
            return loadedBlog;
        }

        public async Task<List<Blog>> GetBlogByDoctorIdAsync(Guid doctorId, int pageNumber, int pageaSize)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId);
            if (doctor == null)
            {
                throw new NotFoundException($"Doctor with id {doctorId} not found.");
            }

            return await _context.Blogs
                .Where(b => b.UserProfileId == doctor.UserProfileId)
                .Skip((pageNumber - 1) * pageaSize)
                .Take(pageaSize)
                .ToListAsync();
        }

        public async Task<List<Blog>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Blogs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var loadedBlog = await _context.Blogs.FindAsync(id);
            if (loadedBlog is null)
            {
                return false;
            }
            return true;
        }

        public async Task<Blog> SaveAsync(Blog entity)
        {
            await _context.Blogs.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Blog> UpdateAsync(Blog entity)
        {
            _context.Blogs.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
