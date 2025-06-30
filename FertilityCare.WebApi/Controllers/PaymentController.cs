using FertilityCare.Infrastructure.Services;
using FertilityCare.UseCase.DTOs.Payments;
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

        public PaymentController(IMomoService momoService, IPaymentService paymentService)
        {
            _momoService = momoService;
            _paymentService = paymentService;
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
        public async Task<ActionResult<ApiResponse<PaymentExecuteResponseDTO>>> CallBackPayment()
        {
            var dto = ParseCallbackPayment(HttpContext.Request.Query);

            var isValid = _momoService.VerifySignatureFromCallback(HttpContext.Request.Query);

            if (!isValid)
            {
                return BadRequest(new ApiResponse<PaymentExecuteResponseDTO>
                {
                    StatusCode = 400,
                    Message = "Invalid signature. This callback is not trusted.",
                    Data = dto,
                    ResponsedAt = DateTime.UtcNow
                });
            }

            await _paymentService.UpdatePayment(dto);

            return Ok(new ApiResponse<PaymentExecuteResponseDTO>
            {
                StatusCode = 200,
                Message = "Payment callback verified",
                Data = dto,
                ResponsedAt = DateTime.UtcNow
            });
        }

        private PaymentExecuteResponseDTO ParseCallbackPayment(IQueryCollection qc)
        {
            return new PaymentExecuteResponseDTO
            {
                Amount = qc["amount"],
                OrderId = qc["orderId"],
                OrderInfo = qc["orderInfo"],
                ResultCode = qc["resultCode"],
                Message = qc.TryGetValue("message", out var msg) ? msg.ToString() : string.Empty,
                ExtraData = qc["extraData"],
                Signature = qc["signature"]
            };
        }
    }

}
