using FertilityCare.UseCase.DTOs.Payments;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Services
{
    public interface IOrderStepPaymentService
    {

        Task<IEnumerable<OrderStepPaymentDTO>> GetOrderStepPaymentsAsync();
   
    }

    public class OrderStepPaymentService : IOrderStepPaymentService
    {
        private readonly IOrderStepPaymentRepository _orderStepPaymentRepository;
        public OrderStepPaymentService(IOrderStepPaymentRepository orderStepPaymentRepository)
        {
            _orderStepPaymentRepository = orderStepPaymentRepository;
        }
        public async Task<IEnumerable<OrderStepPaymentDTO>> GetOrderStepPaymentsAsync()
        {
            var payments = await _orderStepPaymentRepository.FindAllAsync();
            return payments.OrderByDescending(x => x.PaymentDate).Select(payment => payment.MapToOrderStepPaymentDTO());
        }
    }
}
