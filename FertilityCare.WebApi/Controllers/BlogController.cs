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
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
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
        
    }
}
