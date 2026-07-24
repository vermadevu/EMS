using API.DTOs.Asset;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Entities;
using API.Models.Enums;
using AutoMapper;

namespace API.Services;

public class AssetService( IAssetRepository repository, IMapper mapper) : IAssetService
{
    private readonly IAssetRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<AssetDto>> GetAllAsync()
    {
        var assets = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<AssetDto>>(assets);
    }

    public async Task<AssetDto?> GetByIdAsync(int id)
    {
        var asset = await _repository.GetByIdAsync(id);

        if (asset == null)
            return null;

        return _mapper.Map<AssetDto>(asset);
    }

    public async Task<AssetDto> CreateAsync(CreateAssetDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.SerialNumber))
        {
            if (await _repository.ExistsBySerialNumberAsync(dto.SerialNumber))
                throw new Exception("Asset with the same serial number already exists.");
        }

        var asset = _mapper.Map<Asset>(dto);

        asset.AssetCode = await GenerateAssetCodeAsync();
        asset.Status = AssetStatus.Available;
        asset.EmployeeId = null;

        await _repository.AddAsync(asset);

        return _mapper.Map<AssetDto>(asset);
    }

    public async Task<bool> UpdateAsync(int id, UpdateAssetDto dto)
    {
        var asset = await _repository.GetByIdAsync(id);

        if (asset == null)
            return false;

        if (!string.IsNullOrWhiteSpace(dto.SerialNumber))
        {
            if (await _repository.ExistsBySerialNumberAsync(dto.SerialNumber, id))
                throw new Exception("Asset with the same serial number already exists.");
        }

        _mapper.Map(dto, asset);

        await _repository.UpdateAsync(asset);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var asset = await _repository.GetByIdAsync(id);

        if (asset == null)
            return false;

        if (asset.Status == AssetStatus.Assigned)
            throw new Exception("Assigned asset cannot be deleted.");

        await _repository.DeleteAsync(asset);

        return true;
    }

    public async Task<bool> AssignAssetAsync(int assetId, AssignAssetDto dto)
    {
        var asset = await _repository.GetByIdAsync(assetId);

        if (asset == null)
            return false;

        if (asset.Status == AssetStatus.Assigned)
            throw new Exception("Asset is already assigned.");

        if (!await _repository.EmployeeExistsAsync(dto.EmployeeId))
            throw new Exception("Employee not found.");

        asset.EmployeeId = dto.EmployeeId;
        asset.Status = AssetStatus.Assigned;

        await _repository.UpdateAsync(asset);

        return true;
    }

    public async Task<bool> ReturnAssetAsync(int assetId)
    {
        var asset = await _repository.GetByIdAsync(assetId);

        if (asset == null)
            return false;

        if (asset.Status == AssetStatus.Available)
            throw new Exception("Asset is already available.");

        asset.EmployeeId = null;
        asset.Status = AssetStatus.Available;

        await _repository.UpdateAsync(asset);

        return true;
    }

    private async Task<string> GenerateAssetCodeAsync()
    {
        var lastCode = await _repository.GetLastAssetCodeAsync();

        if (string.IsNullOrWhiteSpace(lastCode))
            return "AST0001";

        var number = int.Parse(lastCode.Substring(3));

        return $"AST{number + 1:D4}";
    }
}