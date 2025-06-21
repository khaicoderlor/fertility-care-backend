using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Mappers
{
    public static class ProfileMapper
    {
        public static ProfileDTO MapToProfileDTO(this UserProfile profile)
        {
            return new ProfileDTO
            {
                Id = profile.Id.ToString(),
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                MiddleName = profile.MiddleName,
                FullName = $"{profile.FirstName} {profile.MiddleName} {profile.LastName}".Trim(),
                Gender = profile.Gender.ToString(),
                Address = profile.Address,
                AvatarUrl = profile.AvatarUrl,
                DateOfBirth = profile.DateOfBirth?.ToString("dd/MM/yyyy"),
                CreatedAt = profile.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                UpdatedAt = profile.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
    }
}
