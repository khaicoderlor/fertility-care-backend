using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IEmbryoGainedRepository : IBaseRepository<EmbryoGained, long>
    {
        Task BulkInsertAsync(IEnumerable<EmbryoGained> embryos);
        public Task<IEnumerable<EmbryoGained>> FindByOrderIdAsync(Guid orderId);
    }
}
