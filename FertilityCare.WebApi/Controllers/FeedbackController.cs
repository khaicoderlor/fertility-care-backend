using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Feedbacks;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [Route("api/v1/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
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

    }
}
