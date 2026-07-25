using API.Models.Entities;

namespace API.Interfaces.Repository;

public interface IDocumentRepository
{
    Task<IEnumerable<Document>> GetAllAsync();
    Task<Document?> GetByIdAsync(int id);
    Task<IEnumerable<Document>> GetByEmployeeIdAsync(int employeeId);
    Task AddAsync(Document document);
    Task DeleteAsync(Document document);
    Task<bool> EmployeeExistsAsync(int employeeId);
    Task<Document?> GetByIdAndEmployeeIdAsync(int id, int employeeId);
}