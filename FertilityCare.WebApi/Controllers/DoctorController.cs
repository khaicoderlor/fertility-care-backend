﻿using Fertilitycare.Share.Comon;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebAPI.Controllers
{

    [ApiController]
    [Route("api/v1/doctors")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DoctorDTO>>> GetDoctorById(string id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                if (doctor == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = $"Doctor with ID {id} not found.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                return Ok(new ApiResponse<DoctorDTO>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = doctor,
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

        [HttpGet("{id}/patients")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientInfoTable>>>> GetPatientsByDoctorId(string id)
        {
            try
            {
                var doctor = await _doctorService.GetPatientsByDoctorIdAsync(Guid.Parse(id));

                if (doctor == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = $"Doctor with ID {id} not found.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                return Ok(new ApiResponse<IEnumerable<PatientInfoTable>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = doctor,
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

        [HttpGet("{id}/profiles")]
        public async Task<ActionResult<ApiResponse<DoctorDTO>>> GetDoctorByProfileId(string id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByProfileIdAsync(id);

                if (doctor == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = 404,
                        Message = $"Doctor with ID {id} not found.",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                return Ok(new ApiResponse<DoctorDTO>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = doctor,
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DoctorDTO>>>> GetDoctorsPaged(
        [FromQuery] PaginationRequestDTO request)
        {
            try
            {
                var result = await _doctorService.GetDoctorsPagedAsync(request);

                return Ok(new ApiResponse<IEnumerable<DoctorDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched paged result successfully.",
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

