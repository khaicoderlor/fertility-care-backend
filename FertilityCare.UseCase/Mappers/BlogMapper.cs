﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Blogs;

namespace FertilityCare.UseCase.Mappers
{
    public static class BlogMapper
    {
        //public static Blog MaptoBlog(this BlogDTO blog)
        //{
        //    return new Blog()
        //    {
        //        UserProfileId = Guid.Parse(blog.UserProfileId),
        //        Content = blog.Content,
        //        Status = blog.Status,
        //        CreatedAt = blog.CreatedAt,
        //        UpdatedAt = blog.UpdatedAt,
        //        Title = blog.Title,
        //    };
        //}
        public static BlogDTO MapToBlogDTO(this Blog blog)
        {
            return new BlogDTO()
            {
                Id = blog.Id.ToString(),
                Author = blog.UserProfile.MapToProfileDTO(),
                FullName = blog.UserProfile.FirstName +" "+ blog.UserProfile.MiddleName + " " + blog.UserProfile.LastName,
                Content = blog.Content,
                Status = blog.Status.ToString(),
                Title = blog.Title,
                Category = blog.BlogCategory.ToString(),
                CreatedAt = blog.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss"),
                UpdatedAt = blog.UpdatedAt?.ToString("dd/MM/yyyy hh:mm:ss"),
                ImageUrl = blog.ImageUrl,
                AvatarUrl = blog.UserProfile?.AvatarUrl
            };
        }
    }
}
