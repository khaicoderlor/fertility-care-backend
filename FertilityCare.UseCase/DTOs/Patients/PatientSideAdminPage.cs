using FertilityCare.UseCase.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class PatientSideAdminPage
    {
        public string EmailContact { get; set; } = string.Empty;

        public string PhoneContact { get; set; } = string.Empty;

        public PatientDTO Patient { get; set; } = new PatientDTO();

        public IEnumerable<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
    }
}
