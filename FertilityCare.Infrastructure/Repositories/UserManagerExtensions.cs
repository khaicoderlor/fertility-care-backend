using FertilityCare.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Repositories
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser?> FindByProfileIdAsync(this UserManager<ApplicationUser> userManager, Guid profileId)
        {
            return await userManager.Users
                .FirstOrDefaultAsync(u => u.UserProfileId == profileId);
        }
    }
}
