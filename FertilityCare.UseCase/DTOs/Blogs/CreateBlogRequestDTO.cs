using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Blogs
{
    public class CreateBlogRequestDTO
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string UserProfileId { get; set; }

    }
}
