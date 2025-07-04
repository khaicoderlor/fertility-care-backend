using Fertilitycare.Share.Comon;
using FertilityCare.Infrastructure.Services;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebAPI.Controllers
{

    [ApiController]
    [Route("api/v1/doctors")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        private readonly ICloudStorageService _cloudStorageService;

        public DoctorController(IDoctorService doctorService, ICloudStorageService cloudStorageService)
        {
            _doctorService = doctorService;
            _cloudStorageService = cloudStorageService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DoctorDTO>>> GetDoctorById(string id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                if (doctor == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = $"Doctor with ID {id} not found.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                return Ok(new ApiResponse<DoctorDTO>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = doctor,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet("{id}/patients")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientDashboard>>>> GetPatientsByDoctorId(string id)
        {
            try
            {
                var doctor = await _doctorService.GetPatientsByDoctorIdAsync(Guid.Parse(id));

                if (doctor == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = $"Doctor with ID {id} not found.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                return Ok(new ApiResponse<IEnumerable<PatientDashboard>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = doctor,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet("{id}/profiles")]
        public async Task<ActionResult<ApiResponse<DoctorDTO>>> GetDoctorByProfileId(string id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByProfileIdAsync(id);

                if (doctor == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = $"Doctor with ID {id} not found.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                return Ok(new ApiResponse<DoctorDTO>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = doctor,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DoctorDTO>>>> GetDoctorsPaged(
        [FromQuery] PaginationRequestDTO request)
        {
            try
            {
                var result = await _doctorService.GetDoctorsPagedAsync(request);

                return Ok(new ApiResponse<IEnumerable<DoctorDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched paged result successfully.",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpPatch("{doctorId}/change-avatar")]
        public async Task<ActionResult<ApiResponse<string>>> ChangeAvatarDoctor([FromRoute] string doctorId, [FromForm] IFormFile avatar)
        {
            try
            {
                var secureUrl = await _cloudStorageService.UploadPhotoAsync(avatar);


                if (secureUrl is null)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        StatusCode = 400,
                        Message = "",
                        Data = "Updated failed!",
                        ResponsedAt = DateTime.Now
                    });
                }

                await _doctorService.ChangeAvatarDoctorByIdAsync(Guid.Parse(doctorId), secureUrl);

                return Ok(new ApiResponse<string>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = secureUrl,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet("{doctorId}/recent-patients")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RecentPatientAppointmentDTO>>>> GetRecentPatients([FromRoute] string doctorId)
        {
            try
            {
                if (!Guid.TryParse(doctorId, out var parsedDoctorId))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "Invalid doctorId format. Expecting a valid GUID.",
                        Data = null,
                        ResponsedAt = DateTime.UtcNow
                    });
                }

                var result = await _doctorService.FindTop5RecentPatientsAsync(parsedDoctorId);

                return Ok(new ApiResponse<IEnumerable<RecentPatientAppointmentDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched 5 most recent patients successfully.",
                    Data = result,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return StatusCode(500, new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }
        [HttpPut("{doctorId}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateDoctor([FromRoute] string doctorId, [FromBody] UpdateDoctorRequestDTO request)
        {
            try
            {

                var result = await _doctorService.UpdateDoctorAsync(Guid.Parse(doctorId), request);

                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = $"Doctor with ID {doctorId} not found.",
                        Data = null,
                        ResponsedAt = DateTime.UtcNow
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "Doctor updated successfully.",
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = "Internal server error: " + ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }


    }
}

