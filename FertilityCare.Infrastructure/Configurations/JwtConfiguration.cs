using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations
{
    public class JwtConfiguration
    {
        public const string SectionName = "JwtSettings";

        [Required]
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        public int ExpirationInMinutes { get; set; } = 60;

        public int RefreshTokenExpirationInDays { get; set; } = 7;

    }
}
