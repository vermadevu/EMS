using API.Data;
using API.DTOs.Designation;
using API.Helpers.Pagination;
using API.Interfaces.Repository;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class DesignationRepository(ApplicationDbContext context) : IDesignationRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Designation>> GetAllAsync()
    {
        return await _context.Designations
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<Designation?> GetByIdAsync(int id)
    {
        return await _context.Designations
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Designation designation)
    {
        await _context.Designations.AddAsync(designation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Designation designation)
    {
        _context.Designations.Update(designation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Designation designation)
    {
        _context.Designations.Remove(designation);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Designations
            .AnyAsync(d => d.Name.ToLower() == name.ToLower());
    }

    public async Task<PagedResult<DesignationListItemDto>> GetPagedAsync(DesignationQueryParams queryParams)
    {
        var query = _context.Designations
            .Include(x => x.Employees)
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var search = queryParams.Search.ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(search));
        }

        // Sorting
        query = (queryParams.SortBy.ToLower(), queryParams.SortDirection.ToLower()) switch
        {
            ("name", "desc") =>
                query.OrderByDescending(x => x.Name),
            ("employeecount", "asc") =>
                query.OrderBy(x => x.Employees.Count),
            ("employeecount", "desc") =>
                query.OrderByDescending(x => x.Employees.Count),
            _ =>
                query.OrderBy(x => x.Name)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(x => new DesignationListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                EmployeeCount = x.Employees.Count
            })
            .ToListAsync();

        return new PagedResult<DesignationListItemDto>
        {
            Items = items,
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize,
            TotalCount = totalCount
        };
    }
}