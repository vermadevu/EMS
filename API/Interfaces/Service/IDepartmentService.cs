using API.DTOs.Department;

namespace API.Interfaces.Service;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();

    Task<DepartmentDto?> GetByIdAsync(int id);

    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);

    Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto);

    Task<bool> DeleteAsync(int id);
}