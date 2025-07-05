using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class RecentPatientAppointmentDTO
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string TreatmentName { get; set; } = string.Empty;
        public string LastVisit { get; set; } = string.Empty;
        public string? Status { get; set; }
    }
}
