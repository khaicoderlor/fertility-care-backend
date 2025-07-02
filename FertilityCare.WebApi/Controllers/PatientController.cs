using System.Collections.Generic;
using System.Security.Claims;
using FertilityCare.Infrastructure.Services;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.OrderSteps;
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

        private readonly IPaymentService _paymentService;   

        private readonly ICloudStorageService _cloudStorageService;

        public PatientController(IPatientService patientService, IPatientSecretService patientSecretService, ICloudStorageService cloudStorageService, IPaymentService paymentService)
        {
            _patientService = patientService;
            _patientSecretService = patientSecretService;
            _cloudStorageService = cloudStorageService;
            _paymentService = paymentService;
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

        [HttpGet("{patientId}/contact")]
        public async Task<ActionResult<ApiResponse<PatientInfoContactDTO>>> GetPatientInfoContactByIdAsync([FromRoute] string patientId)
        {
            try
            {
                var result = await _patientSecretService.GetPatientInfoContactByPatientIdAsync(patientId);
                return Ok(new ApiResponse<PatientInfoContactDTO>
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

        [HttpGet("{patientId}/payment-histories")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderStepPaymentTuple>>>> GetPaymentHistories([FromRoute] string patientId)
        {
            try
            {
                var result = await _patientSecretService.GetPaymentHistoriesByPatientId(Guid.Parse(patientId));
                return Ok(new ApiResponse<IEnumerable<OrderStepPaymentTuple>>
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

        [HttpPatch("{patientId}/change-avatar")]
        public async Task<ActionResult<ApiResponse<object>>> UploadAvatarImagebyPatientId([FromRoute] string patientId, [FromForm] IFormFile file)
        {
            try
            {
                var secureUrl = await _cloudStorageService.UploadPhotoAsync(file);

                if(secureUrl is null)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        StatusCode = 400,
                        Message = "",
                        Data = "Updated failed!",
                        ResponsedAt = DateTime.Now
                    });
                }

                await _patientSecretService.UpdateAvatarAsync(patientId, secureUrl);

                return Ok(new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = secureUrl,
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

        [HttpPut("{patientId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateInfoPatientById([FromRoute] string patientId, [FromBody] UpdatePatientInfoDTO request)
        {
            try
            {
               var isUpdated = await _patientService.UpdateInfoPatientByIdAsync(patientId, request);

                if(!isUpdated)
                {
                    return BadRequest(new ApiResponse<bool>
                    {
                        StatusCode = 400,
                        Message = "",
                        Data = isUpdated,
                        ResponsedAt = DateTime.Now
                    });
                }

                return Ok(new ApiResponse<bool>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = isUpdated,
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
