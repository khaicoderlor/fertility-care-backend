using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.EggGained;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/eggs")]
    public class EggGainedController : ControllerBase
    {
        private readonly IEggGainedService _eggGainedService;

        public EggGainedController(IEggGainedService eggGainedService)
        {
            _eggGainedService = eggGainedService;
        }

        [HttpPost("{orderId}")]
        public async Task<ActionResult<ApiResponse<CreateEggResponseDTO>>> CreateEggs(
        [FromRoute] string orderId,
        [FromBody] CreateEggGainedListRequestDTO request)
        {
            if (!Guid.TryParse(orderId, out var parsedOrderId))
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "OrderId không hợp lệ. Vui lòng kiểm tra định dạng.",
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }

            try
            {
                var result = await _eggGainedService.AddEggsAsync(parsedOrderId, request);

                return Ok(new ApiResponse<CreateEggResponseDTO>
                {
                    StatusCode = 200,
                    Message = "Danh sách trứng đã được lưu thành công!",
                    Data = result,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }

        [HttpGet("usable/{orderId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmbryoDropdownEggDTO>>>> GetUsableEggs([FromRoute] string orderId)
        {
            if (!Guid.TryParse(orderId, out var parsedOrderId))
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "OrderId không hợp lệ",
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }

            try
            {
                var result = await _eggGainedService.GetUsableEggsByOrderIdAsync(parsedOrderId);

                return Ok(new ApiResponse<IEnumerable<EmbryoDropdownEggDTO>>
                {
                    StatusCode = 200,
                    Message = "Lấy trứng usable thành công!",
                    Data = result,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }

        [HttpGet("{orderId}/report")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EggReportResponse>>>> GetEggsReportByOrderId([FromRoute] string orderId)
        {
            if (!Guid.TryParse(orderId, out var parsedOrderId))
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "OrderId không hợp lệ",
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }

            try
            {
                var result = await _eggGainedService.GetEggReportByOrderId(parsedOrderId);

                return Ok(new ApiResponse<IEnumerable<EggReportResponse>>
                {
                    StatusCode = 200,
                    Message = "Lấy trứng usable thành công!",
                    Data = result,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }

        [HttpGet]
        [Route("viable/{orderId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EggDataStatistic>>>> GetStatisticEggGradeAndViable([FromRoute] string orderId)
        {
            try
            {
                var result = await _eggGainedService.GetStatisticEggGradeAndViableAsync(Guid.Parse(orderId));
                return Ok(new ApiResponse<IEnumerable<EggDataStatistic>> 
                {
                    StatusCode = 200,
                    Message= "",
                    Data = result,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception e) 
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }


    }
}
