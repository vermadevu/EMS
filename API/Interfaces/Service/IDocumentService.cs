using API.DTOs.Document;
using API.Helpers.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces.Service;

public interface IDocumentService
{
    Task<IEnumerable<DocumentDto>> GetAllAsync();
    Task<DocumentDto?> GetByIdAsync(int id);
    Task<IEnumerable<DocumentDto>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<DocumentDto>> GetMyDocumentsAsync();
    Task<DocumentDto> UploadAsync(UploadDocumentDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteMyDocumentAsync(int id);
    Task<PagedResult<EmployeeDocumentSummaryDto>> GetEmployeeDocumentSummaryAsync(DocumentQueryParams queryParams);
}