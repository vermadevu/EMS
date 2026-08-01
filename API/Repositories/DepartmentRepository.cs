using API.Data;
using API.DTOs.Department;
using API.Helpers.Pagination;
using API.Interfaces.Repository;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class DepartmentRepository(ApplicationDbContext context) : IDepartmentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Department department)
    {
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Departments
            .AnyAsync(d => d.Name.ToLower() == name.ToLower());
    }

    public async Task<int> CountAsync()
    {
        return await _context.Departments.CountAsync();
    }

    public async Task<PagedResult<DepartmentListItemDto>> GetPagedAsync(
    DepartmentQueryParams queryParams)
    {
        var query = _context.Departments
            .Include(x => x.Employees)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var search = queryParams.Search.ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(search));
        }

        query = (queryParams.SortBy.ToLower(),
                 queryParams.SortDirection.ToLower()) switch
        {
            ("name", "desc") => query.OrderByDescending(x => x.Name),
            ("employeecount", "asc") =>
                query.OrderBy(x => x.Employees.Count),
            ("employeecount", "desc") =>
                query.OrderByDescending(x => x.Employees.Count),
            _ => query.OrderBy(x => x.Name)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(x => new DepartmentListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                EmployeeCount = x.Employees.Count
            })
            .ToListAsync();

        return new PagedResult<DepartmentListItemDto>
        {
            Items = items,
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            TotalCount = totalCount
        };
    }
}