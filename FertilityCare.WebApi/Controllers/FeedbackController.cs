using FertilityCare.Infrastructure.Services;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Feedbacks;
using FertilityCare.UseCase.DTOs.Statistics;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [Route("api/v1/feedbacks")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        private readonly IDoctorSecretService _doctorSecretService;

        public FeedbackController(IFeedbackService feedbackService, IDoctorSecretService doctorSecretService)
        {
            _feedbackService = feedbackService;
            _doctorSecretService = doctorSecretService;
        }
        
        [HttpGet("{doctorId}")]
        public async Task<ActionResult<ApiResponse<List<FeedbackDTO>>>> GetAllFeedbackByDoctorId([FromRoute]string doctorId)
        {
            try
            {
                var result = await _feedbackService.GetAllFeedbacksByDoctorIdAsync(doctorId);
                return Ok(new ApiResponse<List<FeedbackDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result,
                    ResponsedAt = DateTime.Now,
                });
            }catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<FeedbackDTO>>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt= DateTime.Now,
                });
            }
        }

        [HttpGet("second/latest/manager-sides")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FeedbackLatestSideManager>>>> GetFeedbackLatestSideManager()
        {
            try
            {
                var result = await _feedbackService.GetSecondFeedbackLatestManagerSide();
                return Ok(new ApiResponse<IEnumerable<FeedbackLatestSideManager>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result,
                    ResponsedAt = DateTime.Now,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<FeedbackDTO>>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now,
                });
            }
        }

        [HttpGet("best-rate")]
        public async Task<ActionResult<ApiResponse<BestRateDoctor?>>> GetBestRateDoctor()
        {
            try
            {
                var result = await _feedbackService.GetBestRateDoctor();
                return Ok(new ApiResponse<BestRateDoctor>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result,
                    ResponsedAt = DateTime.Now,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<FeedbackDTO>>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now,
                });
            }
        }

        [HttpGet("{doctorId}/doctor-sides")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FeedbackSideDoctor>>>> GetFeedbacksSideDoctor([FromRoute] string doctorId)
        {
            try
            {
                var result = await _doctorSecretService.GetFeedbacksOfDoctorSide(Guid.Parse(doctorId));
                return Ok(new ApiResponse<IEnumerable<FeedbackSideDoctor>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result,
                    ResponsedAt = DateTime.Now,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<FeedbackDTO>>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now,
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<FeedbackDTO>>> CreateFeedbackAsync([FromBody] CreateFeedbackRequestDTO request)
        {
            try
            {
                var result = await _feedbackService.CreateFeedbackAsync(request);
                return Ok(new ApiResponse<FeedbackDTO>
                {
                    StatusCode = 200,
                    Message = "Feedback created successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = 404,
                    Message = "Feedback not found",
                    Data = null,
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<FeedbackDTO>>>> GetAllFeedbacksByFilterAsync([FromQuery] FeedbackQueryDTO query)
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacksAsync(query);
                return Ok(new ApiResponse<IEnumerable<FeedbackDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = feedbacks,
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

        [HttpPut("{feedbackId}")]
        public async Task<ActionResult<ApiResponse<FeedbackDTO>>> UpdateFeedbackAsync(string feedbackId, [FromBody] UpdateFeedbackDTO request)
        {
            try
            {
                var result = await _feedbackService.UpdateFeedbackAsync(feedbackId, request);
                return Ok(new ApiResponse<FeedbackDTO>
                {
                    StatusCode = 200,
                    Message = "Feedback updated successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = 404,
                    Message = "Feedback not found",
                    Data = null,
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

        [HttpPut("{feedbackId}/status")]
        public async Task<ActionResult<ApiResponse<FeedbackDTO>>> UpdateFeedbackStatusAsync(string feedbackId, [FromQuery]bool status)
        {
            try
            {
                var result = await _feedbackService.UpdateStatusFeedbackAsync(feedbackId, status);
                return Ok(new ApiResponse<FeedbackDTO>
                {
                    StatusCode = 200,
                    Message = "Feedback status updated successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = 404,
                    Message = "Feedback not found",
                    Data = null,
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
        [HttpGet("full-details")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AllFeedbackDTO>>>> GetAllFeedbackFullDetailsAsync()
        {
            try
            {
                var result = await _feedbackService.GetAllFeedbackAsync();

                return Ok(new ApiResponse<IEnumerable<AllFeedbackDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched all feedbacks with full details successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FeedbackDTO>>>> GetFeedbacksByPatientId([FromRoute] string patientId)
        {
            try
            {
                if (!Guid.TryParse(patientId, out var parsedPatientId))
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        StatusCode = 400,
                        Message = "Invalid patient ID format",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                var result = await _feedbackService.GetFeedbacksByPatientIdAsync(parsedPatientId);

                return Ok(new ApiResponse<IEnumerable<FeedbackDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched feedbacks successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }


    }
}
