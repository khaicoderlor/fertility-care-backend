using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.EmbryoGained;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/embryos")]
public class EmbryoGainedController : ControllerBase
{
    private readonly IEmbryoGainedService _service;

    public EmbryoGainedController(IEmbryoGainedService service)
    {
        _service = service;
    }

    [HttpPost("{orderId}")]
    public async Task<ActionResult<ApiResponse<object>>> CreateEmbryos(
    [FromRoute] string orderId,
    [FromBody] CreateEmbryoGainedListRequestDTO request)
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
            await _service.AddEmbryosAsync(parsedOrderId, request);

            return Ok(new ApiResponse<object>
            {
                StatusCode = 200,
                Message = "Đã tạo danh sách phôi thành công!",
                Data = null,
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

}

