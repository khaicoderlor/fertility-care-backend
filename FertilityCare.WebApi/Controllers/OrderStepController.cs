using FertilityCare.Share.Exceptions;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.OrderSteps;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/steps")]
    public class OrderStepController : ControllerBase
    {
        private readonly IOrderStepService _stepService;

        public OrderStepController(IOrderStepService stepService)
        {
            _stepService = stepService;
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderStepDTO>>> GetStepsByOrderId([FromRoute] string orderId)
        {
            try
            {
                var result = await _stepService.GetAllStepsByOrderIdAsync(Guid.Parse(orderId));
                return Ok(new ApiResponse<IEnumerable<OrderStepDTO>>
                {
                    StatusCode = 200,
                    Message = "Order steps fetched successfully!",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = e.StatusCode,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpPut]
        [Route("{stepId}")]
        public async Task<ActionResult<ApiResponse<StepStatusUpdateResultDTO>>> MarkStatusByStepId([FromRoute] long stepId, [FromQuery] string status)
        {
            try
            {
                var result = await _stepService.MarkStatusByStepIdAsync(stepId, status);
                return Ok(new ApiResponse<StepStatusUpdateResultDTO>
                {
                    StatusCode = 200,
                    Message = "Order step fetched successfully!",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (PreviousNotCompletedExpception e)
            {
                return Ok(new ApiResponse<object>
                {
                    StatusCode = e.StatusCode,
                    Message = "Bước điều trị trước chưa hoàn thành",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (AppointmentNotCompleteException e)
            {
                return Ok(new ApiResponse<object>
                {
                    StatusCode = e.StatusCode,
                    Message = "Các cuộc hẹn của bước điều trị này chưa được hoàn thành!",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotPaidOrderStepException e)
            {
                return Ok(new ApiResponse<object>
                {
                    StatusCode = e.StatusCode,
                    Message = "Bệnh nhân chưa thanh toán bước này không thể hoàn thành",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return Ok(new ApiResponse<object>
                {
                    StatusCode = e.StatusCode,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }
    }
}
