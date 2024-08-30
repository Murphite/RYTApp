namespace RYT.Models.ViewModels
{
    public class UploadImageVM
    {
        public IFormFile? Image { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;        
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
