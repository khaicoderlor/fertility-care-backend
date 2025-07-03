using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Enums;

namespace FertilityCare.UseCase.DTOs.Blogs
{
    public class BlogStatusUpdateRequest
    {
        public BlogStatus Status { get; set; }
    }
}
