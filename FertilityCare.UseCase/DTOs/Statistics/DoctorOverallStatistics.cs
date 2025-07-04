using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Statistics
{
    public class DoctorOverallStatistics
    {

        public long TotalPatients { get; set; }

        public long TotalAppointments { get; set; }

        public decimal TotalRate { get; set; }

        public decimal ComparingPatientsPreviousMonth { get; set; }

        public long TotalPatientsPreviousMonth { get; set; }

        public long TotalPatientsCurrentMonth { get; set; }

        public decimal ComparingAppointmentsPreviousMonth { get; set; }

        public long TotalAppointmentsPreviousMonth { get; set; } 

        public decimal ComparingRatePreviousMonth { get; set; }

        public decimal TotalRatePreviousMonth { get; set; }


    }
}
