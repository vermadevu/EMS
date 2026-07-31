using API.Exceptions;
using API.Helpers;
using API.Interfaces.Service;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

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
            throw new BadRequestException("No file was uploaded.");

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

        var result = await _cloudinary.UploadAsync(uploadParams);

        return GetUploadResult(result);

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

    public async Task<(string PublicId, string Url)> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new BadRequestException("No image was uploaded.");

        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "employee-profile-images",
            PublicId = Guid.NewGuid().ToString(),
            UseFilename = false,
            UniqueFilename = false,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return GetUploadResult(result);
    }

    public async Task DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            return;

        var deleteParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Image
        };

        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Error != null)
            throw new Exception(result.Error.Message);
    }

    private static (string PublicId, string Url) GetUploadResult(UploadResult uploadResult)
    {
        if (uploadResult.Error != null)
            throw new Exception(uploadResult.Error.Message);

        return (
            uploadResult.PublicId,
            uploadResult.SecureUrl.ToString()
        );
    }
}