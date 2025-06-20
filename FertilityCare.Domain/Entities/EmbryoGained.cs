using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities
{
    public class EmbryoGained
    {
        public long Id { get; set; }    

        public EmbryoGrade Grade { get; set; }

        public long EggGainedId { get; set; } 

        public virtual EggGained EggGained { get; set; }

        public EmbryoStatus EmbryoStatus { get; set; }

        public bool IsViable { get; set; }

        public bool IsFrozen { get; set; } = false;

        public bool IsTransfered { get; set; } = false;

        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
