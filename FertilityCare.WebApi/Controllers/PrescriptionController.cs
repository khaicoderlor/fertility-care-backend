using FertilityCare.UseCase.DTOs.Prescriptions;
using FertilityCare.UseCase.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebAPI.Controllers
{
    [Route("api/v1/prescriptions")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }
        [HttpGet("by-patient")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrescriptionDetailDTO>>>> GetPrescriptionByPatientId([FromQuery]string patientId)
        {
            try
            {
                var result = await _prescriptionService.GetPrescriptionByPatientId(patientId);
                return Ok(new ApiResponse<IEnumerable<PrescriptionDetailDTO>>
                {
                    StatusCode = 200,
                    Message = "",
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
