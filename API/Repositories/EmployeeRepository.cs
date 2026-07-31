using API.Constants;
using API.Data;
using API.DTOs;
using API.DTOs.Employee;
using API.Helpers.Pagination;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Entities;
using API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class EmployeeRepository(ApplicationDbContext context) : IEmployeeRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Include(e => e.Manager)
            .OrderBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Include(e => e.Manager)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Employees
            .AnyAsync(e => e.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int excludeId)
    {
        return await _context.Employees
            .AnyAsync(e => e.Email == email && e.Id != excludeId);
    }

    public async Task<bool> ExistsByEmployeeCodeAsync(string employeeCode)
    {
        return await _context.Employees
            .AnyAsync(e => e.EmployeeCode == employeeCode);
    }

    public async Task<string?> GetLastEmployeeCodeAsync()
    {
        return await _context.Employees
            .OrderByDescending(e => e.Id)
            .Select(e => e.EmployeeCode)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DepartmentExistsAsync(int departmentId)
    {
        return await _context.Departments
            .AnyAsync(d => d.Id == departmentId);
    }

    public async Task<bool> DesignationExistsAsync(int designationId)
    {
        return await _context.Designations
            .AnyAsync(d => d.Id == designationId);
    }

    public async Task<bool> ManagerExistsAsync(int managerId)
    {
        return await _context.Employees
            .AnyAsync(e => e.Id == managerId);
    }

    public async Task<bool> HasUserAccountAsync(int employeeId)
    {
        return await _context.Users
        .AnyAsync(u => u.EmployeeId == employeeId);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Employees.CountAsync();
    }

    public async Task<List<Employee>> GetRecentEmployeesAsync(int count = 5)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .OrderByDescending(e => e.JoiningDate)
            .ThenByDescending(e => e.Id) // tie breaker
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Employee>> GetPendingOnboardingAsync(int count = 5)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Where(e => e.Status == EmployeeStatus.Pending)
            .OrderByDescending(e => e.JoiningDate)
            .ThenByDescending(e => e.Id)
            .Take(count)
            .ToListAsync();
    }

    public async Task<PagedResult<Employee>> GetPagedAsync(EmployeeQueryParams query)
    {
        var employees = _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .AsQueryable();

        // Search

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim().ToLower();

            employees = employees.Where(e =>
                e.FirstName.ToLower().Contains(search) ||
                e.LastName.ToLower().Contains(search) ||
                e.EmployeeCode.ToLower().Contains(search) ||
                e.Email.ToLower().Contains(search));
        }

        // Filters

        if (query.DepartmentId.HasValue)
            employees = employees.Where(e =>
                e.DepartmentId == query.DepartmentId);

        if (query.DesignationId.HasValue)
            employees = employees.Where(e =>
                e.DesignationId == query.DesignationId);

        if (query.Status.HasValue)
            employees = employees.Where(e =>
                e.Status == query.Status);

        // Sorting

        employees = (query.SortBy.ToLower(), query.SortDirection.ToLower()) switch
        {
            ("fullname", "asc") => employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName),
            ("fullname", "desc") => employees.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName),
            ("joiningdate", "asc") => employees.OrderBy(e => e.JoiningDate),
            _ => employees.OrderByDescending(e => e.JoiningDate)
        };

        var totalCount = await employees.CountAsync();

        var items = await employees
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<Employee>
        {
            Items = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount,
        };
    }

    public async Task<List<EmployeeListItemDto>> GetManagersAsync()
    {
        var managerRoleId = await _context.Roles
            .Where(r => r.Name == Roles.Manager)
            .Select(r => r.Id)
            .FirstAsync();

        return await (
            from employee in _context.Employees
            join user in _context.Users
                on employee.Id equals user.EmployeeId
            join userRole in _context.UserRoles
                on user.Id equals userRole.UserId
            where userRole.RoleId == managerRoleId
            orderby employee.FirstName, employee.LastName
            select new EmployeeListItemDto
            {
                Id = employee.Id,
                FullName = employee.FirstName + " " + employee.LastName
            }
        ).ToListAsync();
    }
}