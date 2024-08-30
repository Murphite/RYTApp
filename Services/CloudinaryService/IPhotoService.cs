using CloudinaryDotNet.Actions;

namespace RYT.Services.CloudinaryService
{
    public interface IPhotoService
    {
        Task<DeletionResult> DeletePhotoAsync(string publicUrl);

        Task<Dictionary<string, string>> UploadImage(IFormFile photo, string folderName);

    }
}
