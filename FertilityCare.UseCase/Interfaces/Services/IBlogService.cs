using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.Blogs;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IBlogService
    {
        Task<BlogDTO> CreateNewBlog(CreateBlogRequestDTO request);
        Task<BlogDTO> UpdateBlog(string blogId, CreateBlogRequestDTO request);
        Task<List<BlogDTO>> GetBlogByDoctorId(string doctorId);
        Task<List<BlogDTO>> GetAllBlog();

        Task<List<BlogDTO>> GetAllStatusBlog();
        Task<BlogDTO> UpdateStatus(string blogId, string status);
        Task<BlogDTO> UpdateImage(string id, string secureUrl);
    }
}
