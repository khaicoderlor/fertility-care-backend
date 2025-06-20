using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations
{
    public class GoogleAuthConfiguration
    {
        public const string SectionName = "GoogleAuth";

        [Required]
        public string ClientId { get; set; } = string.Empty;

        [Required]
        public string ClientSecret { get; set; } = string.Empty;

    }
}
