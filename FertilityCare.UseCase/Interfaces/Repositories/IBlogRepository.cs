using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Blogs;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IBlogRepository : IBaseRepository<Blog, Guid>
    {
        public Task<List<Blog>> GetBlogByDoctorIdAsync(Guid doctorId);
        public Task<List<Blog>> GetPagedAsync(int pageNumber, int pageSize);
        public Task<List<Blog>> GetAllApprovedAsync();

        Task<IEnumerable<Blog>> GetBlogsByPatientIdAsync(Guid patientId);

        Task SaveChangeAsync();
    }
}
