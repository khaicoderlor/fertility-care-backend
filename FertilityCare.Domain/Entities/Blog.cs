using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Enums;

namespace FertilityCare.Domain.Entities
{
    public class Blog
    {
        public Guid Id { get; set; }
        public Guid UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public BlogStatus Status { get; set; } = BlogStatus.Process;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public string? ImageUrl { get; set; }
    }
}
