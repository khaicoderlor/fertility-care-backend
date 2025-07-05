using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class PatientMonthlyCountDTO
    {
        public int Month { get; set; }
        public long Patients { get; set; }

        public long Appointments { get; set; }
    }
}
