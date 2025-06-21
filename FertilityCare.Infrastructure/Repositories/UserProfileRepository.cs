using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {

        private readonly FertilityCareDBContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileRepository(FertilityCareDBContext context,
            UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        public Task DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserProfile>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfile> FindByIdAsync(Guid id)
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == id);
            if (profile is null)
            {
                throw new NotFoundException($"User profile with ID {id} not found.");
            }

            return profile;
        }

        public async Task<UserProfile?> FindByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.UserProfile;
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == id);
            if(profile is null)
            {
                return false;
            }

            return true;
        }

        public async Task<UserProfile> SaveAsync(UserProfile entity)
        {
            await _context.UserProfiles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<UserProfile> UpdateAsync(UserProfile entity)
        {
            _context.UserProfiles.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
