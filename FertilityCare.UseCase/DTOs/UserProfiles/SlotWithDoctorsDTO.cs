using FertilityCare.UseCase.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.UserProfiles
{
    public class SlotWithDoctorsDTO
    {
        public long SlotId { get; set; }
        public string StartTime { get; set; } 
        public string EndTime { get; set; }   
        public List<DoctorIdDTO> IdSheduleDoctor { get; set; } = new();
    }


}
