using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FertilityCare.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FertilityCare.Infrastructure.Services
{
    public interface ICloudStorageService
    {
        Task<string> UploadPhotoAsync(IFormFile file);

    }

    public class CloudStorageService : ICloudStorageService
    {

        private readonly Cloudinary _cloudinary;

        public CloudStorageService(IOptions<CloudStorageSettings> _config)
        {
            _cloudinary = new Cloudinary(new Account(
                _config.Value.CloudName,
                _config.Value.ApiKey,
                _config.Value.ApiSecret
            ));
        }


        public async Task<string> UploadPhotoAsync(IFormFile file)
        {
            if(file.Length <= 0)
            {
                return null;
            }

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.Name, stream),
                Folder = "avatars",
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }
    }
}
