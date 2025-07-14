using FertilityCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order, Guid>
    {
        Task<IEnumerable<Order>> FindAllByDoctorIdAsync(Guid doctorId);

        Task<IEnumerable<Order>> FindAllByPatientIdAsync(Guid patientId);

        Task SaveChangeAsync();

        Task<int> CountDistinctActivePatientsAsync();

        Task<int> CountDistinctDoctorsAsync();
        Task<int> CountAllOrdersAsync();


    }
}
