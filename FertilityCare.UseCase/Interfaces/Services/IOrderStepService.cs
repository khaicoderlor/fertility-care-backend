using FertilityCare.UseCase.DTOs.OrderSteps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IOrderStepService
    {

        Task<IEnumerable<OrderStepDTO>> GetAllStepsByOrderIdAsync(Guid orderId);

        Task<(OrderStepDTO, string)> MarkStatusByStepIdAsync(long stepId, string status);
    }
}
