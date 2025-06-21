using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;

namespace FertilityCare.UseCase.DTOs.Blogs
{
    public class BlogDTO
    {
        public string UserProfileId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public BlogStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ImageUrl { get; set; }
        public string? AvatarUrl { get; set; }

    }
}
