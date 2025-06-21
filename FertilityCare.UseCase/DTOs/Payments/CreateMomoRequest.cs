using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Payments
{
    public class CreateMomoRequest
    {
        public decimal Amount { get; set; }

        public string OrderId { get; set; }

        public string OrderInfo { get; set; }
    }
}
