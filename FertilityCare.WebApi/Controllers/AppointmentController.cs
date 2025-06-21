using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AppointmentDTO>>>> GetPagedAppointments(
            [FromQuery] AppointmentQueryDTO query)
        {

            try
            {
                var result = await _appointmentService.GetPagedAppointmentsAsync(query);
                return Ok(new ApiResponse<IEnumerable<AppointmentDTO>>
                {
                    StatusCode = 200,
                    Message = "Appointments fetched successfully!",
                    Data = result,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = e.StatusCode,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }

        [HttpPost]
        [Route("{orderId}")]
        public async Task<ActionResult<ApiResponse<AppointmentDTO>>> CreateAppointmentByStepId([FromRoute] string orderId, [FromBody] CreateAppointmentDailyRequestDTO request)
        {
            try
            {
                var result = await _appointmentService.PlaceAppointmentByStepIdAsync(Guid.Parse(orderId), request);
                return Ok(new ApiResponse<AppointmentDTO>
                {
                    StatusCode = 200,
                    Message = "Appointment created successfully",
                    Data = result,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (AppointmentSlotLimitExceededException e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });

            }
        }

        [HttpGet]
        [Route("{orderId}/{stepId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AppointmentDTO>>>> GetAppointmentsByOrderIdAndStepId([FromRoute] string orderId, [FromRoute] long stepId)
        {
            try
            {
                var result = await _appointmentService.GetAppointmentsByStepIdAsync(Guid.Parse(orderId), stepId);
                return Ok(new ApiResponse<IEnumerable<AppointmentDTO>>
                {
                    StatusCode = 200,
                    Message = "Appointments fetched successfully!",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = e.StatusCode,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }

        [HttpPatch]
        [Route("mark-status/{appointmentId}")]
        public async Task<ActionResult<AppointmentDTO>> MarkStatusAppointmentById([FromRoute] string appointmentId, [FromQuery] string status)
        {
            try
            {
                var result = await _appointmentService.MarkStatusAppointmentAsync(Guid.Parse(appointmentId), status);
                return Ok(new ApiResponse<AppointmentDTO>
                {
                    StatusCode = 200,
                    Message = "Marked status successfully!",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = e.StatusCode,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
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
