using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (userProfile is null)
            {
                throw new NotFoundException("User profile not found");
            }
            var doctor = await _doctorRepository.FindByUserProfileIdAsync(Guid.Parse(request.UserProfileId));
            var status = (doctor is null)
                ? BlogStatus.Process : BlogStatus.Approved;

            var blogdto = new BlogDTO()
            {
                Id = Guid.NewGuid().ToString(),
                UserProfileId = request.UserProfileId,
                UserName = userProfile.FirstName + " " + userProfile.MiddleName + " " + userProfile.LastName,
                Content = request.Content,
                Status = status,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                ImageUrl = request.ImageUrl,
                AvatarUrl = userProfile.AvatarUrl
            };
            await _blogRepository.SaveAsync(blogdto.MaptoBlog());
            return blogdto;
        }

        public async Task<List<BlogDTO>> GetBlogByDoctorId(BlogQueryDTO query)
        {
            var blogs = await _blogRepository.GetBlogByDoctorIdAsync(Guid.Parse(query.doctorId), query.PageNumber, query.PageSize);
            return blogs.Select(b => b.MapToBlogDTO()).ToList();
        }
        public async Task<List<BlogDTO>> GetAllBlog(int pageNumber, int pageSize)
        {
            var blogs = await _blogRepository.GetPagedAsync(pageNumber, pageSize);
            return blogs.Select(b => b.MapToBlogDTO()).ToList();
        }
    }
}
