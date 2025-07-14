using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Doctors
{
    public class DoctorSideAdminPage
    {
        public string DoctorEmail { get; set; } = string.Empty;
        public string DoctorPhone { get; set; } = string.Empty;

        public DoctorDTO Doctor { get; set; } = new DoctorDTO();

        public IEnumerable<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
    }
}
