using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Blogs;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.UseCase.Mappers;

namespace FertilityCare.UseCase.Implements
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IDoctorRepository _doctorRepository;
        public BlogService(IBlogRepository blogRepository,
            IUserProfileRepository userProfileRepository,
            IDoctorRepository doctorRepository)
        {
            _blogRepository = blogRepository;
            _userProfileRepository = userProfileRepository;
            _doctorRepository = doctorRepository;
        }
        public async Task<BlogDTO> CreateNewBlog(CreateBlogRequestDTO request)
        {
            var userProfile = await _userProfileRepository.FindByIdAsync(Guid.Parse(request.UserProfileId));

            var doctor = await _doctorRepository.FindByUserProfileIdAsync(Guid.Parse(request.UserProfileId));
            var status = (doctor is null)
                ? BlogStatus.Process : BlogStatus.Approved;

            var blog = new Blog
            {
                Id = Guid.NewGuid(),
                UserProfileId = Guid.Parse(request.UserProfileId),
                Content = request.Content,
                Title = request.Title,
                Status = status,
                ImageUrl = "",
                BlogCategory = GetBlogCategory(request.Topic),
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
            };

            await _blogRepository.SaveAsync(blog);
            return blog.MapToBlogDTO();
        }

        private BlogCategory GetBlogCategory(string topic)
        {
            return topic switch
            {
                "General" => BlogCategory.General,
                "IVF" => BlogCategory.IVF,
                "IUI" => BlogCategory.IUI,
                "InfoFertility" => BlogCategory.InfoFertility,
                "Other" => BlogCategory.Other,
            };
        }

        public async Task<BlogDTO> UpdateImage(string blogId, string secureUrl)
        {
            var blog = await _blogRepository.FindByIdAsync(Guid.Parse(blogId));

            blog.ImageUrl = secureUrl;
            await _blogRepository.SaveChangeAsync();
            return blog.MapToBlogDTO();
        }

        public async Task<List<BlogDTO>> GetBlogByDoctorId(string doctorId)
        {
            var blogs = await _blogRepository.GetBlogByDoctorIdAsync(Guid.Parse(doctorId));
            return blogs.Select(b => b.MapToBlogDTO()).ToList();
        }
        public async Task<List<BlogDTO>> GetAllBlog()
        {
            var blogs = await _blogRepository.GetAllApprovedAsync();
            return blogs.Select(b => b.MapToBlogDTO()).ToList();
        }
        public async Task<BlogDTO> UpdateBlog(string blogId, CreateBlogRequestDTO request)
        {
            var blog = await _blogRepository.FindByIdAsync(Guid.Parse(blogId));
            if (blog is null)
            {
                throw new NotFoundException("Blog not found");
            }
            blog.Title = request.Title;
            blog.BlogCategory = determineBlogCategory(request.Topic);
            blog.Content = request.Content;
            blog.UpdatedAt = DateTime.Now;
            await _blogRepository.SaveChangeAsync();
            return blog.MapToBlogDTO();
        }

        private BlogCategory determineBlogCategory(string category)
        {
            switch (category)
            {
                case "General":
                    return BlogCategory.General;
                case "IVF":
                    return BlogCategory.IVF;
                case "IUI":
                    return BlogCategory.IUI;
                case "InfoFertility":
                    return BlogCategory.InfoFertility;
                case "Other":
                    return BlogCategory.Other;
                default:
                    throw new ArgumentException("Invalid blog category");

            }
        }

        public async Task<BlogDTO> UpdateStatus(string blogId, string status)
        {
            var blog = await _blogRepository.FindByIdAsync(Guid.Parse(blogId));
            blog.Status = GetBlogStatus(status);
            var blogUpdate = await _blogRepository.UpdateAsync(blog);
            return blogUpdate.MapToBlogDTO();
        }

        private BlogStatus GetBlogStatus(string status)
        {
            return status switch
            {
                "Approved" => BlogStatus.Approved,
                "Rejected" => BlogStatus.Rejected,
                "Process" => BlogStatus.Process,
                _ => throw new ArgumentException("Invalid blog status")
            };
        }

        public async Task<IEnumerable<BlogDTO>> GetBlogsByPatientIdAsync(Guid patientId)
        {
            var blogs = await _blogRepository.GetBlogsByPatientIdAsync(patientId);

            foreach (var blog in blogs)
            {
                blog.UserProfile ??= await _userProfileRepository.FindByIdAsync(blog.UserProfileId);
            }

            return blogs.Select(b => b.MapToBlogDTO());
        }


        public async Task<List<BlogDTO>> GetAllStatusBlog()
        {
            var res = await _blogRepository.FindAllAsync();
            return res.Select(b => b.MapToBlogDTO()).ToList();
        }
    }
}
