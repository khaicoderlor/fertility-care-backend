using System.Collections.Generic;
using System.Security.Claims;
using FertilityCare.Infrastructure.Services;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebAPI.Controllers
{
    [Route("api/v1/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        private readonly IPatientSecretService _patientSecretService;

        public PatientController(IPatientService patientService, IPatientSecretService patientSecretService)
        {
            _patientService = patientService;
            _patientSecretService = patientSecretService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientDTO>>>> GetAllAynsc()
        {
            try
            {
                var result = await _patientService.FindAllAsync();
                return Ok(new ApiResponse<IEnumerable<PatientDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = 400,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }

        }

        
    }
}
