using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Blogs
{
    public class BlogQueryDTO
    {
        public string DoctorId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
