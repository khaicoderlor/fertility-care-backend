using FertilityCare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid UserProfileId { get; set; }
        
        public virtual UserProfile? UserProfile { get; set; }

        public string? RefreshToken { get; set; } 

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? GoogleId { get; set; }

        public bool IsGoogleAccount { get; set; }

        public DateTime? LastLogin { get; set; }

        public int FailedLoginAttempts { get; set; }    

        public DateTime LockoutEndTime { get; set; }


    }
}
