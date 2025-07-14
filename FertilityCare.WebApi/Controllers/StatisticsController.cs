using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.Statistics;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [Route("api/v1/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("top-5-doctors/most-appointment")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DoctorDTO>>>> GetTop5DoctorMostAppointment()
        {
            try
            {
                var result = await _statisticsService.GetTop5DoctorMostApointmentAsync();
                return Ok(new ApiResponse<IEnumerable<DoctorDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully",
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

        [HttpGet("doctor/{doctorId}/monthly")]
        public async Task<ActionResult<ApiResponse<List<AverageRateMonthly>>>> GetStatisticAverageRateMonthlyDoctor([FromRoute] string doctorId)
        {
            try
            {
                var result = await _statisticsService.GetStatisticAverageRateMonthlyDoctor(Guid.Parse(doctorId));
                return Ok(new ApiResponse<List<AverageRateMonthly>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully",
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
        [HttpGet("patients-appointments/{doctorId}/monthly")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientMonthlyCountDTO>>>> GetPatientCountByYear([FromRoute] string doctorId, [FromQuery] int year)
        {
            try
            {
                var result = await _statisticsService.GetPatientCountByYearAsync(Guid.Parse(doctorId), year);
                return Ok(new ApiResponse<IEnumerable<PatientMonthlyCountDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
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

        [HttpGet("doctors/{doctorId}/overall")]
        public async Task<ActionResult<ApiResponse<DoctorOverallStatistics>>> GetDoctorOverallStatisticInDashboard([FromRoute] string doctorId)
        {
            try
            {
                var result = await _statisticsService.GetDoctorOverallStatisticInDashboardAsync(doctorId);
                return Ok(new ApiResponse<DoctorOverallStatistics>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
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

        [HttpGet("orders-status/{doctorId}/overall")]
        public async Task<ActionResult<ApiResponse<IEnumerable<StatusTreatmentPatientOverall>>>> GetOrderStatusOverall([FromRoute] string doctorId)
        {
            try
            {
                var result = await _statisticsService.GetOrderStatusOverallByDoctorIdAsync(Guid.Parse(doctorId));
                return Ok(new ApiResponse<IEnumerable<StatusTreatmentPatientOverall>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
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
        [HttpGet("active-patients")]
        public async Task<ActionResult<ApiResponse<string>>> GetCurrentActivePatientCountAsString()
        {
            try
            {
                var countString = await _statisticsService.FindTotalPatientAsync();
                return Ok(new ApiResponse<string>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = countString,
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
        [HttpGet("doctors/active-count")]
        public async Task<ActionResult<ApiResponse<string>>> GetCurrentActiveDoctorCount()
        {
            try
            {
                var countString = await _statisticsService.GetCurrentActiveDoctorCountAsync();
                return Ok(new ApiResponse<string>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = countString,
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
        [HttpGet("orders/count")]
        public async Task<ActionResult<ApiResponse<string>>> GetTotalOrderCount()
        {
            try
            {
                var total = await _statisticsService.CountTotalOrdersAsync();

                return Ok(new ApiResponse<string>
                {
                    StatusCode = 200,
                    Message = "Total number of treatment orders retrieved successfully.",
                    Data = total,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = 400,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }
    }
}
