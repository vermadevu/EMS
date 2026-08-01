using API.Data;
using API.DTOs.Asset;
using API.Helpers.Pagination;
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

    public async Task<int> GetCountAsync()
    {
        return await _context.Assets.CountAsync();
    }

    public async Task<PagedResult<AssetListItemDto>> GetPagedAsync(
    AssetQueryParams queryParams)
    {
        var query = _context.Assets
            .AsNoTracking()
            .Include(a => a.Employee)
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var search = queryParams.Search.Trim().ToLower();

            query = query.Where(a =>
                a.AssetName.ToLower().Contains(search) ||
                a.AssetCode.ToLower().Contains(search) ||
                (a.Brand != null && a.Brand.ToLower().Contains(search)) ||
                (a.Model != null && a.Model.ToLower().Contains(search)) ||
                (a.SerialNumber != null && a.SerialNumber.ToLower().Contains(search)));
        }

        // Asset Type
        if (queryParams.AssetType.HasValue)
        {
            query = query.Where(a =>
                a.AssetType == queryParams.AssetType.Value);
        }

        // Status
        if (queryParams.Status.HasValue)
        {
            query = query.Where(a =>
                a.Status == queryParams.Status.Value);
        }

        // Sorting
        query = (queryParams.SortBy.ToLower(), queryParams.SortDirection.ToLower()) switch
        {
            ("assetcode", "desc") =>
                query.OrderByDescending(a => a.AssetCode),
            ("assetcode", "asc") =>
                query.OrderBy(a => a.AssetCode),
            ("purchasedate", "desc") =>
                query.OrderByDescending(a => a.PurchaseDate),
            ("purchasedate", "asc") =>
                query.OrderBy(a => a.PurchaseDate),
            ("status", "desc") =>
                query.OrderByDescending(a => a.Status),
            ("status", "asc") =>
                query.OrderBy(a => a.Status),
            ("assettype", "desc") =>
                query.OrderByDescending(a => a.AssetType),
            ("assettype", "asc") =>
                query.OrderBy(a => a.AssetType),
            ("assetname", "desc") =>
                query.OrderByDescending(a => a.AssetName),
            _ =>
                query.OrderBy(a => a.AssetName)
        };

        var totalCount = await query.CountAsync();

        var assets = await query
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(a => new AssetListItemDto
            {
                Id = a.Id,
                AssetCode = a.AssetCode,
                AssetName = a.AssetName,
                AssetType = a.AssetType,
                Status = a.Status,
                EmployeeName = a.Employee == null
                    ? null
                    : a.Employee.FullName
            })
            .ToListAsync();

        return new PagedResult<AssetListItemDto>
        {
            Items = assets,
            TotalCount = totalCount,
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize
        };
    }

    public async Task<IEnumerable<Asset>> GetByEmployeeAsync(int employeeId)
    {
        return await _context.Assets
            .Where(x => x.EmployeeId == employeeId)
            .ToListAsync();
    }
}