using API.DTOs.Document;

namespace API.Interfaces.Service;

public interface IDocumentService
{
    Task<IEnumerable<DocumentDto>> GetAllAsync();
    Task<DocumentDto?> GetByIdAsync(int id);
    Task<IEnumerable<DocumentDto>> GetByEmployeeIdAsync(int employeeId);
    Task<DocumentDto> UploadAsync(UploadDocumentDto dto);
    Task<bool> DeleteAsync(int id);
}