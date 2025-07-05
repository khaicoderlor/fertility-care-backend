using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class PatientMonthlyCountDTO
    {
        public string Month { get; set; } = string.Empty;
        public long Patients { get; set; }

        public long Appointments { get; set; }
    }
}
