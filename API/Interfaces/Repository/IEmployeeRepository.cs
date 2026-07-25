using API.Models.Entities;

namespace API.Interfaces.Repository;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email, int excludeId);
    Task<bool> ExistsByEmployeeCodeAsync(string employeeCode);
    Task<string?> GetLastEmployeeCodeAsync();
    Task<bool> DepartmentExistsAsync(int departmentId);
    Task<bool> DesignationExistsAsync(int designationId);
    Task<bool> ManagerExistsAsync(int managerId);
    Task<bool> HasUserAccountAsync(int employeeId);
}