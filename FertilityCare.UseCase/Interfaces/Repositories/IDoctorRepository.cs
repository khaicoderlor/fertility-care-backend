using Fertilitycare.Share.Pagination;
using FertilityCare.Domain.Entities;
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

        Task<Doctor> FindByUserProfileIdAsync(Guid userProfile);
    }
}
