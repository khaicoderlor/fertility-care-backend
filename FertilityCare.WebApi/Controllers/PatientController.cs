using System.Collections.Generic;
using System.Security.Claims;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebAPI.Controllers
{
    [Route("api/v1/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        private readonly IPatientSecretService _patientSecretService;

        public PatientController(IPatientService patientService, IPatientSecretService patientSecretService)
        {
            _patientService = patientService;
            _patientSecretService = patientSecretService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientDTO>>>> GetAllAynsc()
        {
            try
            {
                var result = await _patientService.FindAllAsync();
                return Ok(new ApiResponse<IEnumerable<PatientDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = 400,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }

        }

        [HttpGet("{patientId}")]
        public async Task<ActionResult<ApiResponse<PatientDTO>>> GetPatientByIdAsync([FromRoute]string patientId)
        {
            try
            {
                var result = await _patientService.FindPatientByPatientIdAsync(patientId);
                return Ok(new ApiResponse<PatientDTO>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = 400,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpPost("me")]
        public async Task<ActionResult<ApiResponse<PatientSecretInfo>>> GetPatientSecretInfo()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var patientInfo = await _patientSecretService.GetPatientByUserIdAsync(id);
                return Ok(new ApiResponse<PatientSecretInfo>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = patientInfo,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException)
            {
                return Ok(new ApiResponse<PatientSecretInfo>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = new PatientSecretInfo
                    {
                        PatientId = null,
                        UserProfileId = null,
                        OrderIds = null,
                    },
                    ResponsedAt = DateTime.Now
                });
            }
            catch(Exception e)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = 400,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
           
        }
    }
}
