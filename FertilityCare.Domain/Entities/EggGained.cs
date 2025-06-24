using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities
{
    public class EggGained
    {
        public long Id { get; set; }

        public EggGrade Grade { get; set; } 

        public bool IsUsable { get; set; }

        public DateOnly DateGained { get; set; }    

        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
