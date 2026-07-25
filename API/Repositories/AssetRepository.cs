using API.Data;
using API.Interfaces.Repository;
using API.Models.Entities;
using API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class AssetRepository(ApplicationDbContext context) : IAssetRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Asset>> GetAllAsync()
    {
        return await _context.Assets
            .Include(a => a.Employee)
            .ToListAsync();
    }

    public async Task<Asset?> GetByIdAsync(int id)
    {
        return await _context.Assets
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Asset asset)
    {
        await _context.Assets.AddAsync(asset);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Asset asset)
    {
        _context.Assets.Update(asset);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Asset asset)
    {
        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsBySerialNumberAsync(string serialNumber)
    {
        return await _context.Assets
            .AnyAsync(a => a.SerialNumber == serialNumber);
    }

    public async Task<bool> ExistsBySerialNumberAsync(string serialNumber, int excludeId)
    {
        return await _context.Assets
            .AnyAsync(a =>
                a.SerialNumber == serialNumber &&
                a.Id != excludeId);
    }

    public async Task<string?> GetLastAssetCodeAsync()
    {
        return await _context.Assets
            .OrderByDescending(a => a.Id)
            .Select(a => a.AssetCode)
            .FirstOrDefaultAsync();
    }

    public async Task<Asset?> GetAvailableAssetAsync(int id)
    {
        return await _context.Assets
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.Status == AssetStatus.Available);
    }
}