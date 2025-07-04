using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class RecentPatientAppointmentDTO
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string TreatmentServiceName { get; set; }
        public DateOnly LastAppointmentDate { get; set; }
        public DateOnly? NextAppointmentDate { get; set; }
    }
}
