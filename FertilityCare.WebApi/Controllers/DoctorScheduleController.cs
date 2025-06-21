using Fertilitycare.Share.Comon;
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DoctorScheduleDTO>>> CreateScheduleAsync([FromBody] CreateDoctorScheduleRequestDTO request)
        {
            try
            {
                var result = await _doctorScheduleService.CreateScheduleAsync(request);
                return Ok(new ApiResponse<DoctorScheduleDTO>
                {
                    StatusCode = 201,
                    Message = "Schedule created successfully.",
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteScheduleAsync(long id)
        {
            try
            {
                await _doctorScheduleService.DeleteScheduleAsync(id);
                return Ok(new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "Schedule deleted successfully.",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
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

        [HttpPut]
        public async Task<ActionResult<ApiResponse<DoctorScheduleDTO>>> UpdateScheduleAsync([FromBody] UpdateDoctorScheduleRequestDTO request)
        {
            try
            {
                var result = await _doctorScheduleService.UpdateScheduleAsync(request);
                return Ok(new ApiResponse<DoctorScheduleDTO>
                {
                    StatusCode = 200,
                    Message = "Schedule updated successfully.",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
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

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DoctorScheduleDTO>>>> GetPagedSchedulesAsync([FromQuery] PaginationRequestDTO request)
        {
            try
            {
                var result = await _doctorScheduleService.GetSchedulesPagedAsync(request);
                return Ok(new ApiResponse<IEnumerable<DoctorScheduleDTO>>
                {
                    StatusCode = 200,
                    Message = "Paged schedules fetched successfully.",
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

        [HttpGet]
        [Route("slots/{doctorId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SlotWithScheduleDTO>>>> GetSlotsWithSchedulesByDateAndDoctor(
        [FromQuery] string date,
        [FromRoute] string doctorId)
        {
            var result = await _doctorScheduleService.GetSlotWithDoctorsByDateAsync(date, doctorId);

            return Ok(new ApiResponse<IEnumerable<SlotWithScheduleDTO>>
            {
                StatusCode = 200,
                Message = "Slots with schedules fetched successfully.",
                Data = result,
                ResponsedAt = DateTime.Now
            });
        }

        [HttpPost("recurring")]
        public async Task<ActionResult<ApiResponse<object>>> CreateRecurringScheduleAsync([FromBody] CreateRecurringScheduleRequestDTO request)
        {
            try
            {
                if (request.DoctorId == Guid.Empty)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "DoctorId is required.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                if (request.StartDate == null || request.EndDate == null)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "StartDate and EndDate are required.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                if (request.EndDate < request.StartDate)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "EndDate must be after StartDate.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                if (request.WorkingDays == null || !request.WorkingDays.Any())
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "At least one WorkingDay must be selected.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                if (request.SlotIds == null || !request.SlotIds.Any())
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        StatusCode = 400,
                        Message = "At least one SlotId must be selected.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                await _doctorScheduleService.CreateRecurringScheduleAsync(request);

                return Ok(new ApiResponse<object>
                {
                    StatusCode = 201,
                    Message = "Recurring schedule created successfully.",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
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
