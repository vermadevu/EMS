namespace API.Interfaces.Service
{
    public interface ICloudinaryService
    {
        Task<(string PublicId, string Url)> UploadDocumentAsync(IFormFile file);
        Task DeleteDocumentAsync(string publicId);
    }
}
