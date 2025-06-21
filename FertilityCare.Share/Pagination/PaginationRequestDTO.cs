using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fertilitycare.Share.Comon
{
    public class PaginationRequestDTO
    {
        public int MaxPageSize { get; set; } = 10;

        public int PageSize { get; set; } = 3;

        public int Page { get; set; } = 1;

    }
}
