using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace RYT.Services.CloudinaryService
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary cloudinary;

        public PhotoService(IConfiguration config)
        {
            var cloudName = config.GetSection("Cloudinary:CloudName").Value;
            var apiKey = config.GetSection("Cloudinary:ApiKey").Value;
            var apiSecret = config.GetSection("Cloudinary:ApiSecret").Value;

            Account account = new Account
            {
                ApiKey = apiKey,

                ApiSecret = apiSecret,
                Cloud = cloudName
            };

            cloudinary = new Cloudinary(account);
        }

        public async Task<Dictionary<string, string>> UploadImage(IFormFile photo, string folderName)
        {
            var response = new Dictionary<string, string>();
            var defaultSize = 500000;
            var allowedTypes = new List<string>() { "jpeg", "jpg", "png" };
            Console.WriteLine($"Allowed Types: {string.Join(", ", allowedTypes)}");


            if (photo == null)
            {
                response.Add("Code", "400");
                response.Add("Message", "No file uploaded");
                return response;
            }

            var file = photo;

            if (file.Length < 1 || file.Length > defaultSize)
            {
                response.Add("Code", "400");
                response.Add("Message", "Invalid size");
                return response;
            }

            if (allowedTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            {
                response.Add("Code", "400");
                response.Add("Message", "Invalid type");
                return response;
            }

            var uploadResult = new ImageUploadResult();

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face"),
                    Folder = folderName
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }

            if (!string.IsNullOrEmpty(uploadResult.PublicId))
            {
                response.Add("Code", "200");
                response.Add("Message", "Upload successful");
                response.Add("PublicId", uploadResult.PublicId);
                response.Add("Url", uploadResult.Url.ToString());

                return response;
            }

            response.Add("Code", "400");
            response.Add("Message", "Failed to upload");
            return response;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
        {
            var publicId = publicUrl.Split('/').Last().Split('.')[0];
            var deleteParams = new DeletionParams(publicId);
            return await cloudinary.DestroyAsync(deleteParams);
        }

    }
}
