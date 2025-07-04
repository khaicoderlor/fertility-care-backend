﻿using FertilityCare.Shared.Exceptions;
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
    }
}
