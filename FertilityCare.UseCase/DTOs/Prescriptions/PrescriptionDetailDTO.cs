using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Orders;
using FertilityCare.UseCase.DTOs.PrescriptionItem;

namespace FertilityCare.UseCase.DTOs.Prescriptions
{
    public class PrescriptionDetailDTO
    {
        public string Id { get; set; } = string.Empty;

        public OrderDTO Order { get; set; } = new OrderDTO();
        
        public string PrescriptionDate { get; set; }

        public List<PrescriptionItemDTO> PrescriptionItems { get; set; } = new ();
    }
}
