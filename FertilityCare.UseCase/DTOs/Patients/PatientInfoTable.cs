using FertilityCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class PatientInfoTable
    {
        public string PatientId { get; set; }

        public string PatientName { get; set; }

        public string OrderId { get;set; }

        public string StartDate { get; set; }   

        public string? EndDate { get; set; }

        public string Status { get; set; }

        public long? TotalEggs { get; set; } 

    }
}
