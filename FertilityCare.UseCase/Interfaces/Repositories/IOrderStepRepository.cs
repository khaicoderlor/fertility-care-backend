using FertilityCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IOrderStepRepository : IBaseRepository<OrderStep, long>
    {
        Task SaveChangeAsync();

        Task<IEnumerable<OrderStep>> SaveAllAsync(IEnumerable<OrderStep> orderSteps);

        Task<IEnumerable<OrderStep>> FindAllByOrderIdAsync(Guid orderId);
    }
}
