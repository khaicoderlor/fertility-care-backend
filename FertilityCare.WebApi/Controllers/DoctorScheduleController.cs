using Fertilitycare.Share.Comon;
using Fertilitycare.Share.Pagination;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.DoctorSchedules;
using FertilityCare.UseCase.DTOs.Slots;
using FertilityCare.UseCase.DTOs.UserProfiles;
using FertilityCare.UseCase.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/doctor-schedules")]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorScheduleService _doctorScheduleService;

        public DoctorScheduleController(IDoctorScheduleService doctorScheduleService)
        {
            _doctorScheduleService = doctorScheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DoctorScheduleDTO>>>> FindAllSchedulesAsync()
        {
            try
            {
                var result = await _doctorScheduleService.FindAllSchedulesAsync();
                return Ok(new ApiResponse<IEnumerable<DoctorScheduleDTO>>
                {
                    StatusCode = 200,
                    Message = "Schedules fetched successfully.",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet("by-doctor")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DoctorScheduleDTO>>>> GetAllSchedulesAsync([FromQuery] Guid doctorId)
        {
            try
            {
                var result = await _doctorScheduleService.GetAllSchedulesAsync(doctorId);
                return Ok(new ApiResponse<IEnumerable<DoctorScheduleDTO>>
                {
                    StatusCode = 200,
                    Message = "Schedules fetched successfully.",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DoctorScheduleDTO>>> GetScheduleByIdAsync(long id)
        {
            try
            {
                var result = await _doctorScheduleService.GetScheduleByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = "Schedule not found.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }
                return Ok(new ApiResponse<DoctorScheduleDTO>
                {
                    StatusCode = 200,
                    Message = "Schedule fetched successfully.",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }


    }
}
