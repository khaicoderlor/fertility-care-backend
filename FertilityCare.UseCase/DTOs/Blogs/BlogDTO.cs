using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.UserProfiles;

namespace FertilityCare.UseCase.DTOs.Blogs
{
    public class BlogDTO
    {
        public string Id { get; set; }
        public ProfileDTO Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string FullName { get; set; }
        public string Status { get; set; }

        public string Category { get; set; }
        public string CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public string? ImageUrl { get; set; }
        public string? AvatarUrl { get; set; }

    }
}
