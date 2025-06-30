using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.EmbryoGained;
using FertilityCare.UseCase.DTOs.Embryos;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers;

[ApiController]
[Route("api/v1/embryos")]
public class EmbryoGainedController : ControllerBase
{
    private readonly IEmbryoGainedService _service;

    public EmbryoGainedController(IEmbryoGainedService service)
    {
        _service = service;
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<ApiResponse<List<EmbryoData>>>> GetEmbryosByOrderIdAsync(
        [FromRoute] string orderId)
    {
        try
        {
            var embryos = await _service.GetEmbryosByOrderIdAsync(orderId);
            return Ok(new ApiResponse<List<EmbryoData>>
            {
                StatusCode = 200,
                Message = "Lấy danh sách phôi thành công!",
                Data = embryos,
                ResponsedAt = DateTime.UtcNow
            });
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiResponse<List<EmbryoData>>
            {
                StatusCode = 404,
                Message = e.Message,
                Data = null,
                ResponsedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<List<EmbryoData>>
            {
                StatusCode = 400,
                Message = ex.Message,
                Data = null,
                ResponsedAt = DateTime.UtcNow
            });
        }
    }

    [HttpGet("usable/{orderId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<EmbryoData>>>> GetEmbryosUsableByOrderIdAsync(
        [FromRoute] string orderId)
    {
        try
        {
            var embryos = await _service.GetEmbryosUsableByOrderIdAsync(orderId);
            return Ok(new ApiResponse<IEnumerable<EmbryoData>>
            {
                StatusCode = 200,
                Message = "Load embryos success",
                Data = embryos,
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
    public async Task<ActionResult<ApiResponse<IEnumerable<EmbryoReportResponse>>>> GetEmbryoReportByOrderIdAsync(
        [FromRoute] string orderId)
    {
        try
        {
            var embryos = await _service.GetEmbryosReportByOrderIdAsync(Guid.Parse(orderId));
            return Ok(new ApiResponse<IEnumerable<EmbryoReportResponse>>
            {
                StatusCode = 200,
                Message = "Load embryos successfully!",
                Data = embryos,
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

