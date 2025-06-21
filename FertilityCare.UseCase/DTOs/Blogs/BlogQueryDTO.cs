using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Blogs
{
    public class BlogQueryDTO
    {
        public string doctorId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
