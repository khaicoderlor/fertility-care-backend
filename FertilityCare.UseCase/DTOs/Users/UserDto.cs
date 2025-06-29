﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Users
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;

        public string ProfileId { get; set; } = string.Empty;

        public string PatientId { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string AvatarUrl { get; set; }=  string.Empty;

        public List<string>? OrderIds { get; set; }
    }
}
