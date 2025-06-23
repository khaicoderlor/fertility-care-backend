using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Orders
{
    public class OrderInfo
    {
        public string Id { get; set; }

        public string DoctorName { get; set; }

        public string PatientName { get; set; } 

        public string TreatmentServiceName { get; set; }    

        public string StartDate { get; set; }

        public string? EndDate { get; set; } 

        public string Status { get; set; }

        public bool IsFrozen { get; set; }

        public long? TotalEggs { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
