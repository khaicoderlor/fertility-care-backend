using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Auths
{
    public class SyncApplicationUser
    {
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public string TreatmentMethod { get; set; } = string.Empty;

        public DateTime LastLogin { get; set; }

        public string AvatarUrl { get; set; } = string.Empty;

    }
}
