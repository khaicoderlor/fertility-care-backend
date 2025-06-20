using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IBaseRepository<T, TKey> where T: class
    {

        Task<T> FindByIdAsync(TKey id);
        Task<IEnumerable<T>> FindAllAsync();
        Task<T> SaveAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteByIdAsync(TKey id);
        Task<bool> IsExistAsync(TKey id);

    }
}
