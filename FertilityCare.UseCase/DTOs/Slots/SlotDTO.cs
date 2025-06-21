using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Slots
{
    public class SlotDTO
    {
        public long Id { get; set; }

        public int SlotNumber { get; set; }

        public string? StartTime { get; set; }   

        public string? EndTime { get; set; }

        public string? CreatedAt { get; set; }

        public string ? UpdatedAt { get; set; }

    }
}
