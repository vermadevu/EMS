using API.DTOs.Department;
using API.Helpers.Pagination;
using API.Models.Entities;

namespace API.Interfaces.Repository;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(Department department);
    Task<bool> ExistsByNameAsync(string name);
    Task<int> CountAsync();
    Task<PagedResult<DepartmentListItemDto>> GetPagedAsync(DepartmentQueryParams queryParams);

}