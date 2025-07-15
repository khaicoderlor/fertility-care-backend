using FertilityCare.UseCase.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.DoctorSchedules
{
    public class DoctorScheduleSideManager
    {
        public DoctorDTO doctor { get; set; }

        public List<DoctorScheduleDTO> schedules { get; set; }

    }
}
