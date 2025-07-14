using FertilityCare.UseCase.DTOs.TreatmentServices;
using FertilityCare.UseCase.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebAPI.Controllers
{
    [Route("api/v1/treatments")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        private readonly IPublicTreatmentService _publicTreatmentService;
        public TreatmentController(IPublicTreatmentService publicTreatmentService)
        {
            _publicTreatmentService = publicTreatmentService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TreatmentServiceDTO>>>> GetAll()
        {
            try
            {
                var result = await _publicTreatmentService.GetAllAsync();
              
                return Ok(new ApiResponse<IEnumerable<TreatmentServiceDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<TreatmentServiceDTO>>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<TreatmentServiceDTO>>> Update([FromBody] TreatmentServiceDTO treatmentServiceDTO)
        {
            try
            {
                var result = await _publicTreatmentService.UpdateAsync(treatmentServiceDTO);
                return Ok(new ApiResponse<TreatmentServiceDTO>
                {
                    StatusCode = 200,
                    Message = "Treatment service updated successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
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
