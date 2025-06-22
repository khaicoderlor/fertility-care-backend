using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.EmbryoTransfers;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [Route("api/v1/embryotransfer")]
    [ApiController]
    public class EmbryoTransferController : ControllerBase
    {
        private readonly IEmbryoTransferService _embryoTransferService;
        public EmbryoTransferController(IEmbryoTransferService embryoTransferService)
        {
            _embryoTransferService = embryoTransferService;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmbryoTransferDTO>>> CreateEmbryoTransferAsync([FromBody] CreateEmbryoTransferRequestDTO request, [FromQuery] bool isFrozen)
        {
            try
            {
                var result = await _embryoTransferService.CreateEmbryoTransferAsync(request, isFrozen);
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
        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> ReTransferAsync([FromBody] CreateEmbryoReTransferRequestDTO request)
        {
            try
            {
                var result = await _embryoTransferService.ReTransferAsync(request);
                return Ok(new ApiResponse<bool>
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
                return BadRequest(new ApiResponse<bool>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = false,
                    ResponsedAt = DateTime.Now
                });
            }
        }


    }
}
