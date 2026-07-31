using API.DTOs.Document;
using API.Helpers.Pagination;
using API.Models.Entities;

namespace API.Interfaces.Repository;

public interface IDocumentRepository
{
    Task<IEnumerable<Document>> GetAllAsync();
    Task<Document?> GetByIdAsync(int id);
    Task<IEnumerable<Document>> GetByEmployeeIdAsync(int employeeId);
    Task AddAsync(Document document);
    Task DeleteAsync(Document document);
    Task<Document?> GetByIdAndEmployeeIdAsync(int id, int employeeId);
    Task<PagedResult<EmployeeDocumentSummaryDto>> GetEmployeeDocumentSummaryAsync(DocumentQueryParams queryParams);
}