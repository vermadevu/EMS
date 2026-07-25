using API.Data;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Entities;
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
}