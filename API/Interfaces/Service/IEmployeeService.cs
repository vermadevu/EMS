using API.DTOs;
using API.DTOs.Employee;
using API.Helpers.Pagination;

namespace API.Interfaces.Service;

public interface IEmployeeService
{
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> CompleteOnboardingAsync();
    Task<bool> ActivateEmployeeAsync(int id);
    Task<PagedResult<EmployeeListItemDto>> GetPagedAsync(EmployeeQueryParams query);
    Task<IEnumerable<EmployeeListItemDto>> GetManagersAsync();
}