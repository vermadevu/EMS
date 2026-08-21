using API.DTOs.Document;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Mapping;
using API.Models.Entities;
using API.Models.Enums;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace API.Tests.Services;

public class DocumentServiceTests
{
    private readonly Mock<IDocumentRepository> _repository = new();
    private readonly Mock<ICloudinaryService> _cloudinary = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();
    private readonly DocumentService _service;

    public DocumentServiceTests()
    {
        var mapper = new MapperConfiguration(configuration =>
            configuration.AddProfile<MappingProfile>()).CreateMapper();

        _service = new DocumentService(
            _repository.Object,
            _cloudinary.Object,
            mapper,
            _currentUser.Object);
    }

    [Fact]
    public async Task UploadAsync_ShouldStoreCloudMetadataAndReturnDocument()
    {
        var file = CreateFile("resume.pdf", "application/pdf", "resume content");
        var request = new UploadDocumentDto
        {
            EmployeeId = 7,
            DocumentType = DocumentType.Resume,
            File = file
        };
        _cloudinary.Setup(service => service.UploadDocumentAsync(file))
            .ReturnsAsync(("documents/resume", "https://cloudinary.test/resume"));
        _repository.Setup(repository => repository.AddAsync(It.IsAny<Document>()))
            .Callback<Document>(document => document.Id = 12)
            .Returns(Task.CompletedTask);
        _repository.Setup(repository => repository.GetByIdAsync(12))
            .ReturnsAsync(() => new Document
            {
                Id = 12,
                OriginalFileName = "resume.pdf",
                PublicId = "documents/resume",
                Url = "https://cloudinary.test/resume",
                FileSize = file.Length,
                EmployeeId = 7,
                DocumentType = DocumentType.Resume,
                Employee = CreateEmployee(7)
            });

        var result = await _service.UploadAsync(request);

        Assert.Equal("resume.pdf", result.OriginalFileName);
        Assert.Equal("documents/resume", result.PublicId);
        Assert.Equal("https://cloudinary.test/resume", result.Url);
        _repository.Verify(repository => repository.AddAsync(It.Is<Document>(document =>
            document.EmployeeId == request.EmployeeId &&
            document.DocumentType == request.DocumentType &&
            document.ContentType == file.ContentType &&
            document.FileSize == file.Length)), Times.Once);
    }

    [Fact]
    public async Task UploadMyDocumentAsync_ShouldUseCurrentEmployeeId()
    {
        var file = CreateFile("id.png", "image/png", "image");
        _currentUser.Setup(service => service.EmployeeId).Returns(15);
        _cloudinary.Setup(service => service.UploadDocumentAsync(file))
            .ReturnsAsync(("documents/id", "https://cloudinary.test/id"));
        _repository.Setup(repository => repository.AddAsync(It.IsAny<Document>()))
            .Callback<Document>(document => document.Id = 20)
            .Returns(Task.CompletedTask);
        _repository.Setup(repository => repository.GetByIdAsync(20))
            .ReturnsAsync(() => new Document
            {
                Id = 20,
                OriginalFileName = "id.png",
                EmployeeId = 15,
                Employee = CreateEmployee(15)
            });

        await _service.UploadMyDocumentAsync(new UploadDocumentDto
        {
            EmployeeId = 999,
            DocumentType = DocumentType.Photo,
            File = file
        });

        _repository.Verify(repository => repository.AddAsync(It.Is<Document>(document =>
            document.EmployeeId == 15)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCloudFileAndDatabaseDocument()
    {
        var document = CreateDocument(3, 8, "documents/contract");
        _repository.Setup(repository => repository.GetByIdAsync(document.Id)).ReturnsAsync(document);

        var result = await _service.DeleteAsync(document.Id);

        Assert.True(result);
        _cloudinary.Verify(service => service.DeleteDocumentAsync(document.PublicId), Times.Once);
        _repository.Verify(repository => repository.DeleteAsync(document), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalseWithoutDeletingCloudFile_WhenDocumentDoesNotExist()
    {
        _repository.Setup(repository => repository.GetByIdAsync(404)).ReturnsAsync((Document?)null);

        var result = await _service.DeleteAsync(404);

        Assert.False(result);
        _cloudinary.Verify(service => service.DeleteDocumentAsync(It.IsAny<string>()), Times.Never);
        _repository.Verify(repository => repository.DeleteAsync(It.IsAny<Document>()), Times.Never);
    }

    [Fact]
    public async Task DeleteMyDocumentAsync_ShouldOnlyDeleteDocumentBelongingToCurrentEmployee()
    {
        _currentUser.Setup(service => service.EmployeeId).Returns(15);
        var document = CreateDocument(3, 15, "documents/own");
        _repository.Setup(repository => repository.GetByIdAndEmployeeIdAsync(document.Id, 15))
            .ReturnsAsync(document);

        var result = await _service.DeleteMyDocumentAsync(document.Id);

        Assert.True(result);
        _cloudinary.Verify(service => service.DeleteDocumentAsync(document.PublicId), Times.Once);
        _repository.Verify(repository => repository.DeleteAsync(document), Times.Once);
    }

    [Fact]
    public async Task DeleteMyDocumentAsync_ShouldReturnFalse_WhenDocumentBelongsToAnotherEmployee()
    {
        _currentUser.Setup(service => service.EmployeeId).Returns(15);
        _repository.Setup(repository => repository.GetByIdAndEmployeeIdAsync(3, 15))
            .ReturnsAsync((Document?)null);

        var result = await _service.DeleteMyDocumentAsync(3);

        Assert.False(result);
        _cloudinary.Verify(service => service.DeleteDocumentAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetMyDocumentsAsync_ShouldQueryCurrentEmployee()
    {
        _currentUser.Setup(service => service.EmployeeId).Returns(15);
        _repository.Setup(repository => repository.GetByEmployeeIdAsync(15))
            .ReturnsAsync(new[] { CreateDocument(1, 15, "documents/own") });

        var result = await _service.GetMyDocumentsAsync();

        Assert.Single(result);
        _repository.Verify(repository => repository.GetByEmployeeIdAsync(15), Times.Once);
    }

    private static FormFile CreateFile(string fileName, string contentType, string contents)
    {
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(contents));
        return new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    private static Employee CreateEmployee(int id)
    {
        return new Employee
        {
            Id = id,
            FirstName = "Test",
            LastName = "Employee",
            Email = "employee@dems.test"
        };
    }

    private static Document CreateDocument(int id, int employeeId, string publicId)
    {
        return new Document
        {
            Id = id,
            EmployeeId = employeeId,
            PublicId = publicId,
            OriginalFileName = "document.pdf",
            Url = "https://cloudinary.test/document",
            Employee = CreateEmployee(employeeId)
        };
    }
}