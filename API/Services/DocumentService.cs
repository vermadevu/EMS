using API.DTOs.Document;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using AutoMapper;

namespace API.Services;

public class DocumentService( IDocumentRepository repository, ICloudinaryService cloudinaryService, IMapper mapper) : IDocumentService
{
    private readonly IDocumentRepository _repository = repository;
    private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DocumentDto>> GetAllAsync()
    {
        var documents = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<DocumentDto>>(documents);
    }

    public async Task<DocumentDto?> GetByIdAsync(int id)
    {
        var document = await _repository.GetByIdAsync(id);

        if (document == null)
            return null;

        return _mapper.Map<DocumentDto>(document);
    }

    public async Task<IEnumerable<DocumentDto>> GetByEmployeeIdAsync(int employeeId)
    {
        if (!await _repository.EmployeeExistsAsync(employeeId))
            throw new Exception("Employee not found.");

        var documents = await _repository.GetByEmployeeIdAsync(employeeId);

        return _mapper.Map<IEnumerable<DocumentDto>>(documents);
    }

    public async Task<DocumentDto> UploadAsync(UploadDocumentDto dto)
    {
        if (!await _repository.EmployeeExistsAsync(dto.EmployeeId))
            throw new Exception("Employee not found.");

        var uploadResult = await _cloudinaryService.UploadDocumentAsync(dto.File);

        var document = new Document
        {
            OriginalFileName = dto.File.FileName,
            PublicId = uploadResult.PublicId,
            Url = uploadResult.Url,
            ContentType = dto.File.ContentType,
            FileSize = dto.File.Length,
            EmployeeId = dto.EmployeeId,
            DocumentType = dto.DocumentType
        };

        await _repository.AddAsync(document);

        return _mapper.Map<DocumentDto>(document);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var document = await _repository.GetByIdAsync(id);

        if (document == null)
            return false;

        await _cloudinaryService.DeleteDocumentAsync(document.PublicId);

        await _repository.DeleteAsync(document);

        return true;
    }
}