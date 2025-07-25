﻿using FertilityCare.Infrastructure.Services;
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
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BlogDTO>>>> GetAllBlogs([FromQuery]int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var blogs = await _blogService.GetAllBlog(pageNumber, pageSize);
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
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BlogDTO>>>> GetBlogByDoctorId(string doctorId, [FromQuery] int pageNumher, [FromQuery] int pageSize)
        {
            try
            {
                var blogs = await _blogService.GetBlogByDoctorId(new BlogQueryDTO
                {
                    DoctorId = doctorId,
                    PageNumber = pageNumher,
                    PageSize = pageSize
                });
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
        public async Task<ActionResult<ApiResponse<BlogDTO>>> CreateNewBlog([FromBody] CreateBlogRequestDTO request, [FromBody] IFormFile file)
        {
            try
            {
                var blog = await _blogService.CreateNewBlog(request);
                if(file != null && file.Length > 0)
                {
                    var secureUrl = await _cloudStorageService.UploadPhotoAsync(file);
                    blog = await _blogService.UpdateImage(blog.Id, secureUrl);
                }
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

        [HttpPut("{blogId}")]
        public async Task<ActionResult<ApiResponse<BlogDTO>>> UpdateBlog(string blogId, [FromBody] CreateBlogRequestDTO request)
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
        [HttpPut("{blogId}/status")]
        public async Task<ActionResult<ApiResponse<BlogDTO>>> UpdateStatus(string blogId, [FromBody] BlogStatusUpdateRequest status)
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
