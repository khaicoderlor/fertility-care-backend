using FertilityCare.Infrastructure.Identity;
using FertilityCare.UseCase.DTOs.Auths;
using FertilityCare.UseCase.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Services
{
    public interface IAccountService
    {

        Task<IEnumerable<AccountSideAdmin>> GetAccountSideAdmins();

    }

    public class AccountService : IAccountService
    {
        public readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<AccountSideAdmin>> GetAccountSideAdmins()
        {
            var users = await _userManager.Users.ToListAsync();

            var accountSideAdmins = new List<AccountSideAdmin>();
            foreach (var user in users)
            {
                accountSideAdmins.Add(new AccountSideAdmin
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    LastLogin = user.LastLogin?.ToString("dd/MM/yyyy") ?? " - ",
                    IsGoogleAccount = user.IsGoogleAccount,
                    LockoutEnabled = user.LockoutEnabled,
                    Profile = user.UserProfile.MapToProfileDTO(),
                    CreatedAt = user.UserProfile.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss")
                });
            }

            return accountSideAdmins;
        }
    }
}
