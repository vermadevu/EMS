using API.Data;
using API.DTOs.Document;
using API.Helpers.Pagination;
using API.Interfaces.Repository;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class DocumentRepository(ApplicationDbContext context): IDocumentRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _context.Documents.
                Include(d => d.Employee)
                .ToListAsync();
        }

        public async Task<Document?> GetByIdAsync(int id)
        {
            return await _context.Documents
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Document>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Documents
                .Where(d => d.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task AddAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Document document)
        {
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
        }

        public async Task<Document?> GetByIdAndEmployeeIdAsync(int id, int employeeId)
        {
            return await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == id && d.EmployeeId == employeeId);
        }

        public async Task<PagedResult<EmployeeDocumentSummaryDto>> GetEmployeeDocumentSummaryAsync(
            DocumentQueryParams queryParams)
        {
            var query = _context.Employees
                .AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.ToLower();

                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(search) ||
                    e.LastName.ToLower().Contains(search) ||
                    e.EmployeeCode.ToLower().Contains(search));
            }

            // Sorting
            query = (queryParams.SortBy.ToLower(), queryParams.SortDirection.ToLower()) switch
            {
                ("department", "asc") => query.OrderBy(e => e.Department.Name),
                ("department", "desc") => query.OrderByDescending(e => e.Department.Name),
                ("documents", "asc") => query.OrderBy(e => e.Documents.Count()),
                ("documents", "desc") => query.OrderByDescending(e => e.Documents.Count()),
                ("fullname", "desc") => query.OrderByDescending(e => e.FirstName)
                                             .ThenByDescending(e => e.LastName),
                _ => query.OrderBy(e => e.FirstName)
                          .ThenBy(e => e.LastName)
            };

            var totalCount = await query.CountAsync();

            var employees = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(e => new EmployeeDocumentSummaryDto
                {
                    EmployeeId = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FirstName + " " + e.LastName,
                    ProfileImage = e.ProfileImage,
                    Department = e.Department.Name,
                    TotalDocuments = e.Documents.Count()
                })
                .ToListAsync();

            return new PagedResult<EmployeeDocumentSummaryDto>
            {
                Items = employees,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }
    }
}
