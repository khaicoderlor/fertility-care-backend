using FertilityCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IDoctorScheduleRepository : IBaseRepository<DoctorSchedule, long>
    {
        public Task<IQueryable<DoctorSchedule>> FindAllQueryableAsync();

        public Task<IEnumerable<DoctorSchedule>> GetSchedulesByDateAndDoctorAsync(string workDate, string doctorId);

        public Task BulkInsertAsync(IEnumerable<DoctorSchedule> schedules);
    }
}
