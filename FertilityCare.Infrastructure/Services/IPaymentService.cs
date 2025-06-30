using FertilityCare.Domain.constants;
using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.UseCase.DTOs.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Services
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentAsync(CreatePaymentRequestDTO dto);

        Task UpdatePayment(PaymentExecuteResponseDTO paymentExecute);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IMomoService _momoService;

        private readonly FertilityCareDBContext _context;

        public PaymentService(FertilityCareDBContext context, IMomoService momoService)
        {
            _context = context;
            _momoService = momoService;
        }

        public async Task<string> CreatePaymentAsync(CreatePaymentRequestDTO dto)
        {
            var paymentCode = $"{dto.TreatmentName}-{Guid.NewGuid().ToString().ToUpper().Substring(0,8)}";

            var paymentStep = new OrderStepPayment
            {
                PatientId = dto.PatientId,
                OrderStepId = dto.OrderStepId,
                PaymentCode = paymentCode,
                TotalAmount = dto.TotalAmount,
                PaymentMethod = dto.PaymentMethod.Equals("Momo", StringComparison.OrdinalIgnoreCase)
                ?PaymentMethod.Momo:PaymentMethod.Cashier,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.Now
            };

            _context.orderStepPayments.Add(paymentStep);
            await _context.SaveChangesAsync();

            var payUrl = await _momoService.CreatePaymentAsync(new CreateMomoRequest
            {
                Amount = dto.TotalAmount,
                OrderId = paymentCode,
                OrderInfo = dto.OrderInfo ?? "Thanh toán đơn hàng IVF"
            });

            return payUrl;
        }

        public async Task UpdatePayment(PaymentExecuteResponseDTO paymentExecute)
        {
            var loadedPayment = await _context.orderStepPayments.Where(x => x.PaymentCode == paymentExecute.OrderId).FirstOrDefaultAsync();

            if (loadedPayment == null)
            {
                throw new Exception("Không tìm thấy thông tin thanh toán.");
            }

            if (loadedPayment.Status == PaymentStatus.Paid)
            {
                return;
            }

            loadedPayment.GatewayResponseCode = paymentExecute.ResultCode;
            loadedPayment.GatewayMessage = paymentExecute.Message;

            if (paymentExecute.ResultCode.Equals(MomoCallbackResult.PAID))
            {
                loadedPayment.Status = PaymentStatus.Paid;
            }
            else if(paymentExecute.ResultCode.Equals(MomoCallbackResult.CANCELLED))
            {
                loadedPayment.Status = PaymentStatus.Cancelled;
            } 
            else
            {
                loadedPayment.Status = PaymentStatus.Failed;
            }

            await _context.SaveChangesAsync();
        }
    }
}
