using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Appointments
{
    public class AppointmentDataTable
    {
        public string Id { get; set; }

        public string DoctorName { get; set; }  

        public string Specialization { get; set; }

        public string AppointmentDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string TreatmentServiceName { get; set; }

        public string Target { get; set; }

        public string TreatmentStepName { get; set; }

        public decimal ExtraFee { get; set; }

        public string Note { get; set; }

        public string AppointmentStatus { get; set; }
    }
}
