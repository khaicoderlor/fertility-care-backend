using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.EmbryoTransfers;
using FertilityCare.UseCase.DTOs.OrderSteps;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [Route("api/v1/transfers")]
    [ApiController]
    public class EmbryoTransferController : ControllerBase
    {
        private readonly IEmbryoTransferService _embryoTransferService;

        public EmbryoTransferController(IEmbryoTransferService embryoTransferService)
        {
            _embryoTransferService = embryoTransferService;
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmbryoTransferDTO>>> CreateEmbryoTransferAsync([FromBody] CreateEmbryoTransferRequestDTO request)
        {
            try
            {
                var result = await _embryoTransferService.CreateEmbryoTransferAsync(request);
                return Ok(new ApiResponse<EmbryoTransferDTO>
                {
                    StatusCode = 200,
                    Message = "Embryo Transfer successfully.",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = "Embryo Transfer Not Found.",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<EmbryoTransferDTO>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpPut("{orderId}")]
        public async Task<ActionResult<ApiResponse<bool>>> MakeStatusOrderIsFrozenAsync([FromRoute] string orderId, [FromQuery] bool isAccept)
        {
            try
            {
                var result = await _embryoTransferService.MakeStatusOrderIsFrozen(orderId, isAccept);
                return Ok(new ApiResponse<bool>
                {
                    StatusCode = 200,
                    Message = "Update status order successfully.",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = "Order Not Found.",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = false,
                    ResponsedAt = DateTime.Now
                });
            }
        }


        [HttpPatch("{orderId}")]
        public async Task<ActionResult<ApiResponse<bool>>> ReTransferAsync([FromRoute]string orderId)
        {
            try
            {
                var result = await _embryoTransferService.ReTransferAsync(orderId);
                return Ok(new ApiResponse<OrderStepDTO>
                {
                    StatusCode = 200,
                    Message = "Re-transfer successfully.",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = "Re-transfer Not Found.",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<OrderStepDTO>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet("{orderId}/report")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmbryoTransferredReportResponse>>>> GetEmbryoTransferReportByOrderId([FromRoute] string orderId)
        {
            try
            {
                var result = await _embryoTransferService.GetEmbryoTransferReportByOrderIdAsync(Guid.Parse(orderId));
                return Ok(new ApiResponse<IEnumerable<EmbryoTransferredReportResponse>>
                {
                    StatusCode = 200,
                    Message = "Re-transfer successfully.",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = "Re-transfer Not Found.",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<OrderStepDTO>
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
