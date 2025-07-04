using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IDoctorRepository : IBaseRepository<Doctor, Guid>
    {
        Task<IQueryable<Doctor>> GetQueryableAsync();

        Task<IEnumerable<Doctor>> GetPagedAsync(int pageNumber, int pageSize);

        Task<Doctor?> FindByUserProfileIdAsync(Guid userProfile);
        Task<Doctor> FindByProfileIdAsync(string id);
        Task SaveChangeAsync();
    }
}
