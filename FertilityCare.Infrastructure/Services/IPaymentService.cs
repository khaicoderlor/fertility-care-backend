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

    public sealed class PaymentService : IPaymentService
    {
        private readonly FertilityCareDBContext _db;
        private readonly IMomoService _momoService;

        public PaymentService(FertilityCareDBContext db, IMomoService momoService)
        {
            _db = db;
            _momoService = momoService;
        }

        public async Task<string> CreatePaymentAsync(CreatePaymentRequestDTO dto)
        {
            var guid = Guid.NewGuid().ToString("N"); 
            var orderId = $"IVF-{guid.Substring(0, 8)}"; 

            var payment = new OrderStepPayment
            {
                PatientId = dto.PatientId,
                OrderStepId = dto.OrderStepId,
                PaymentCode = orderId,
                TotalAmount = dto.TotalAmount,
                PaymentMethod = dto.PaymentMethod.Equals("momo", StringComparison.OrdinalIgnoreCase)
                                 ? PaymentMethod.Momo
                                 : PaymentMethod.Cashier,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };

            _db.orderStepPayments.Add(payment);
            await _db.SaveChangesAsync();

            if (payment.PaymentMethod == PaymentMethod.Momo)
            {
                var payUrl = await _momoService.CreatePaymentAsync(new CreateMomoRequest
                {
                    Amount = dto.TotalAmount,
                    OrderId = orderId,
                    OrderInfo = dto.OrderInfo ?? "Thanh toán đơn hàng IVF",
                    ExtraData = dto.ExtraData ?? string.Empty
                });

                return payUrl;
            }

            return string.Empty;
        }

        public async Task UpdatePayment(PaymentExecuteResponseDTO dto)
        {
            var payment = await _db.orderStepPayments.FirstOrDefaultAsync(p => p.PaymentCode == dto.OrderId);
            if (payment is null) throw new InvalidOperationException("Không tìm thấy thông tin thanh toán.");

            payment.GatewayResponseCode = dto.ResultCode;
            payment.GatewayMessage = dto.Message;


            payment.Status = dto.ResultCode switch
            {
                "0" => PaymentStatus.Paid,
                "1006" => PaymentStatus.Cancelled,
                _ => PaymentStatus.Failed
            };

            if(payment.Status == PaymentStatus.Paid)
            {
                payment.OrderStep.PaymentStatus = PaymentStatus.Paid;
            } 
            else if (payment.Status == PaymentStatus.Cancelled)
            {
                payment.OrderStep.PaymentStatus = PaymentStatus.Cancelled;
            }
            else
            {
                payment.OrderStep.PaymentStatus = PaymentStatus.Failed;
            }

            payment.PaymentDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }

}
