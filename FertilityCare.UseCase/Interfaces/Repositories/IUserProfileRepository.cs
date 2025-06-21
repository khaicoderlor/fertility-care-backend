using FertilityCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IUserProfileRepository : IBaseRepository<UserProfile, Guid>
    {

        public Task SaveChangeAsync();

        public Task<UserProfile?> FindByUserIdAsync(string userId);
    }
}
