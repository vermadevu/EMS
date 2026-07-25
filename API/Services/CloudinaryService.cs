using API.Helpers;
using API.Interfaces.Service;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> options)
    {
        var settings = options.Value;

        var account = new Account(
            settings.CloudName,
            settings.ApiKey,
            settings.ApiSecret);

        _cloudinary = new Cloudinary(account);
    }

    public async Task<(string PublicId, string Url)> UploadDocumentAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("No file was uploaded.");

        using var stream = file.OpenReadStream();

        var uploadParams = new RawUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "employee-documents",
            PublicId = $"{Guid.NewGuid()}",
            UseFilename = false,
            UniqueFilename = false,
            Overwrite = false
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
            throw new Exception(uploadResult.Error.Message);

        return
        (
            uploadResult.PublicId,
            uploadResult.SecureUrl.ToString()
        );
    }

    public async Task DeleteDocumentAsync(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            return;

        var deleteParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Raw
        };

        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Error != null)
            throw new Exception(result.Error.Message);
    }
}