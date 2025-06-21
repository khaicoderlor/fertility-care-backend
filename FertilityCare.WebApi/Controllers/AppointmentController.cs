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



    }
}
