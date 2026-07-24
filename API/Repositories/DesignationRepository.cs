using API.Data;
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
}