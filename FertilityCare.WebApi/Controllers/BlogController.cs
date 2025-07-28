using FertilityCare.Infrastructure.Services;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Blogs;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [Route("api/v1/blog")]
    [ApiController] 
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
     
        private readonly ICloudStorageService _cloudStorageService;

        public BlogController(IBlogService blogService, ICloudStorageService cloudStorageService)
        {
            _blogService = blogService;
            _cloudStorageService = cloudStorageService;
        }
        //[HttpGet]
        //public async Task<ActionResult<ApiResponse<IEnumerable<BlogDTO>>>> GetAllBlogs([FromQuery] int pageNumber, [FromQuery] int pageSize)
        //{
        //    try
        //    {
        //        var blogs = await _blogService.GetAllBlog(pageNumber, pageSize);
        //        return Ok(new ApiResponse<IEnumerable<BlogDTO>>
        //        {
        //            StatusCode = 200,
        //            Message = "Fetched successfully.",
        //            Data = blogs,
        //            ResponsedAt = DateTime.Now
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new ApiResponse<object>
        //        {
        //            StatusCode = 500,
        //            Message = e.Message,
        //            Data = null,
        //            ResponsedAt = DateTime.Now
        //        });
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BlogDTO>>>> GetAllApprovedBlogs()
        {
            try
            {
                var blogs = await _blogService.GetAllBlog();
                return Ok(new ApiResponse<IEnumerable<BlogDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = blogs,
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

        [HttpGet("all-status")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BlogDTO>>>> GetAllBlogs()
        {
            try
            {
                var blogs = await _blogService.GetAllStatusBlog();
                return Ok(new ApiResponse<IEnumerable<BlogDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = blogs,
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

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BlogDTO>>>> GetBlogsByPatientId([FromRoute] string patientId)
        {
            try
            {
                if (!Guid.TryParse(patientId, out var parsedId))
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        StatusCode = 400,
                        Message = "Invalid patientId format",
                        Data = null,
                        ResponsedAt = DateTime.Now
                    });
                }

                var result = await _blogService.GetBlogsByPatientIdAsync(parsedId);
                return Ok(new ApiResponse<IEnumerable<BlogDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched blogs by patientId successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>
                {
                    StatusCode = 404,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }


        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BlogDTO>>>> GetBlogByDoctorId([FromRoute]string doctorId)
        {
            try
            {
                var blogs = await _blogService.GetBlogByDoctorId(doctorId);
                return Ok(new ApiResponse<IEnumerable<BlogDTO>>
                {
                    StatusCode = 200,
                    Message = "Fetched successfully.",
                    Data = blogs,
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse<BlogDTO>>> CreateNewBlog([FromBody] CreateBlogRequestDTO request)
        {
            try
            {
                var blog = await _blogService.CreateNewBlog(request);
                //if (file != null && file.Length > 0)
                //{
                //    var secureUrl = await _cloudStorageService.UploadPhotoAsync(file);
                //    blog = await _blogService.UpdateImage(blog.Id, secureUrl);
                //}
                return Ok(new ApiResponse<BlogDTO>
                {
                    StatusCode = 200,
                    Message = "Created successfully.",
                    Data = blog,
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

        [HttpPut("{blogId}/content")]
        public async Task<ActionResult<ApiResponse<BlogDTO>>> UpdateBlog([FromRoute] string blogId, [FromBody] CreateBlogRequestDTO request)
        {
            try
            {
                var blog = await _blogService.UpdateBlog(blogId, request);
                return Ok(new ApiResponse<BlogDTO>
                {
                    StatusCode = 200,
                    Message = "Updated successfully.",
                    Data = blog,
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
        [HttpPut("{blogId}")]
        public async Task<ActionResult<ApiResponse<BlogDTO>>> UpdateStatus([FromRoute] string blogId, [FromQuery] string status)
        {
            try
            {
                var blog = await _blogService.UpdateStatus(blogId, status);
                return Ok(new ApiResponse<BlogDTO>
                {
                    StatusCode = 200,
                    Message = "Status updated successfully.",
                    Data = blog,
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
