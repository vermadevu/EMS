using API.Exceptions;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Xunit;

namespace API.Tests.Services;

public class CloudinaryServiceTests
{
    private readonly CloudinaryService _service = new(Options.Create(new CloudinarySettings
    {
        CloudName = "test-cloud",
        ApiKey = "test-key",
        ApiSecret = "test-secret"
    }));

    [Fact]
    public async Task UploadDocumentAsync_ShouldRejectEmptyFile()
    {
        var file = new FormFile(new MemoryStream(), 0, 0, "file", "empty.pdf");

        await Assert.ThrowsAsync<BadRequestException>(() => _service.UploadDocumentAsync(file));
    }

    [Fact]
    public async Task UploadImageAsync_ShouldRejectEmptyFile()
    {
        var file = new FormFile(new MemoryStream(), 0, 0, "file", "empty.png");

        await Assert.ThrowsAsync<BadRequestException>(() => _service.UploadImageAsync(file));
    }

    [Fact]
    public async Task UploadDocumentAsync_ShouldRejectNullFile()
    {
        await Assert.ThrowsAsync<BadRequestException>(() => _service.UploadDocumentAsync(null!));
    }

    [Fact]
    public async Task DeleteOperations_ShouldIgnoreBlankPublicIds()
    {
        await _service.DeleteDocumentAsync(" ");
        await _service.DeleteImageAsync(string.Empty);
    }
}