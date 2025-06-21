using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.UserProfiles
{
    public class ProfileDTO
    {
        public string? Id { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public string? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }

        public string? CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }
    }
}
