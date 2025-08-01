﻿using FertilityCare.Infrastructure.Services;
using FertilityCare.UseCase.DTOs.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace FertilityCare.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderStepPaymentService _orderStepPaymentService;

        public PaymentController(IMomoService momoService, IPaymentService paymentService, IOrderStepPaymentService orderStepPaymentService)
        {
            _momoService = momoService;
            _paymentService = paymentService;
            _orderStepPaymentService = orderStepPaymentService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> CreatePayment([FromBody] CreatePaymentRequestDTO request)
        {
            var payUrl = await _paymentService.CreatePaymentAsync(request);
            return Ok(new ApiResponse<string>
            {
                StatusCode = 200,
                Message = "",
                Data = payUrl,
                ResponsedAt = DateTime.UtcNow
            });
        }

        [HttpPost("callback")]
        public async Task<ActionResult<ApiResponse<PaymentExecuteResponseDTO>>> CallBackPayment([FromBody] Dictionary<string, string> parameters)
        {
            try
            {
                var dto = new PaymentExecuteResponseDTO
                {
                    Amount = parameters.GetValueOrDefault("amount"),
                    OrderId = parameters.GetValueOrDefault("orderId"),
                    OrderInfo = parameters.GetValueOrDefault("orderInfo"),
                    ResultCode = parameters.GetValueOrDefault("resultCode"),
                    Message = parameters.GetValueOrDefault("message"),
                    ExtraData = parameters.GetValueOrDefault("extraData"),
                    Signature = parameters.GetValueOrDefault("signature")
                };

                var isValid = _momoService.VerifySignatureFromCallback(parameters);

                if (!isValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "Invalid signature.",
                        Data = null,
                        ResponsedAt = DateTime.UtcNow
                    });
                }

                await _paymentService.UpdatePayment(dto);

                return Ok(new ApiResponse<PaymentExecuteResponseDTO>
                {
                    StatusCode = 200,
                    Message = "Verified from return url.",
                    Data = dto,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = "Error: " + ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }

        [HttpGet("admin-sides")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderStepPaymentDTO>>>> GetAllPayments()
        {
            try
            {
                var payments = await _orderStepPaymentService.GetOrderStepPaymentsAsync();
                return Ok(new ApiResponse<IEnumerable<OrderStepPaymentDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = payments,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }

    }

}
