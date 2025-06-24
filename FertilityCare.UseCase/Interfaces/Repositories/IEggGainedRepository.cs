using FertilityCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IEggGainedRepository
    {
        Task BulkInsertAsync(IEnumerable<EggGained> eggs);
        Task<IEnumerable<EggGained>> FindAllByOrderIdAsync(Guid orderId);
        Task<IEnumerable<EggGained>> GetUsableEggsByOrderIdAsync(Guid orderId);

    }
}
