using FertilityCare.UseCase.DTOs.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Auths
{
    public class AccountSideAdmin
    {
        public string Id { get; set; }

        public bool IsGoogleAccount { get; set; }   

        public string CreatedAt { get; set; }

        public string? LastLogin { get; set; }

        public string? Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool LockoutEnabled { get; set; }

        public ProfileDTO Profile { get; set; }
    }
}
