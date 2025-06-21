using FertilityCare.UseCase.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IOrderService
    {

        Task<OrderDTO?> PlaceOrderAsync(CreateOrderRequestDTO request);

        Task<OrderDTO> GetOrderByIdAsync(Guid orderId);

        Task<long?> SetTotalEgg(long totalEgg, string orderId);


        Task<IEnumerable<OrderDTO>> GetOrderByDoctorIdAsync(Guid doctorId);

        Task<IEnumerable<OrderDTO>> GetOrderByPatientIdAsync(Guid patientId);


        
    }
}
