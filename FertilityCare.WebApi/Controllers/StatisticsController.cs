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

        [HttpGet("/doctors/{doctorId}/overall")]
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

        [HttpGet("/orders-status/{doctorId}/overall")]
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
    }
}
