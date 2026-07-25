using API.Data;
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

        public async Task<bool> EmployeeExistsAsync(int employeeId)
        {
            return await _context.Documents
                .AnyAsync(e => e.Id == employeeId);
        }

        public async Task<Document?> GetByIdAndEmployeeIdAsync(int id, int employeeId)
        {
            return await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == id && d.EmployeeId == employeeId);
        }
    }
}
