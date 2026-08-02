using API.DTOs.Asset;
using API.Exceptions;
using API.Helpers.Pagination;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Entities;
using API.Models.Enums;
using AutoMapper;

namespace API.Services;

public class AssetService( IAssetRepository repository, IMapper mapper, IEmployeeRepository employeeRepository, ICurrentUserService currentUserService) : IAssetService
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IAssetRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<IEnumerable<AssetDto>> GetAllAsync()
    {
        var assets = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<AssetDto>>(assets);
    }

    public async Task<AssetDto> GetByIdAsync(int id)
    {
        var asset = await _repository.GetByIdAsync(id) ?? throw new NotFoundException("Asset not found.");

        return _mapper.Map<AssetDto>(asset);
    }

    public async Task<AssetDto> CreateAsync(CreateAssetDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.SerialNumber))
        {
            if (await _repository.ExistsBySerialNumberAsync(dto.SerialNumber))
                throw new BadRequestException("Asset with the same serial number already exists.");
        }

        var asset = _mapper.Map<Asset>(dto);

        asset.AssetCode = await GenerateAssetCodeAsync();
        asset.Status = AssetStatus.Available;
        asset.EmployeeId = null;

        await _repository.AddAsync(asset);

        return _mapper.Map<AssetDto>(asset);
    }

    public async Task<AssetDto> UpdateAsync(int id, UpdateAssetDto dto)
    {
        var asset = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Asset not found.");

        if (!string.IsNullOrWhiteSpace(dto.SerialNumber) &&
            await _repository.ExistsBySerialNumberAsync(dto.SerialNumber, id))
        {
            throw new BadRequestException(
                "Asset with the same serial number already exists.");
        }

        _mapper.Map(dto, asset);

        await _repository.UpdateAsync(asset);

        return _mapper.Map<AssetDto>(asset);
    }

    public async Task DeleteAsync(int id)
    {
        var asset = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Asset not found.");

        if (asset.Status == AssetStatus.Assigned)
        {
            throw new BadRequestException(
                "Assigned assets cannot be deleted.");
        }

        await _repository.DeleteAsync(asset);
    }

    public async Task AssignAsync(int assetId, AssignAssetDto dto)
    {
        var asset = await _repository.GetAvailableAssetAsync(assetId) ?? throw new BadRequestException("Asset is not available.");

        var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId) ?? throw new NotFoundException("Employee not found.");
        
        if (employee.Status != EmployeeStatus.Active)
        {
            throw new BadRequestException(
                "Assets can only be assigned to active employees.");
        }

        asset.EmployeeId = employee.Id;
        asset.Status = AssetStatus.Assigned;

        await _repository.UpdateAsync(asset);
    }

    public async Task ReturnAsync(int assetId)
    {
        var asset = await _repository.GetByIdAsync(assetId) ?? throw new NotFoundException("Asset not found.");

        if (asset.Status == AssetStatus.Available)
        {
            throw new BadRequestException("Asset is already available.");
        }

        asset.EmployeeId = null;
        asset.Status = AssetStatus.Available;

        await _repository.UpdateAsync(asset);
    }

    private async Task<string> GenerateAssetCodeAsync()
    {
        var lastCode = await _repository.GetLastAssetCodeAsync();

        if (string.IsNullOrWhiteSpace(lastCode))
            return "AST0001";

        var number = int.Parse(lastCode.Substring(3));

        return $"AST{number + 1:D4}";
    }

    public async Task<PagedResult<AssetListItemDto>> GetPagedAsync(AssetQueryParams queryParams)
    {
        return await _repository.GetPagedAsync(queryParams);
    }

    public async Task<IEnumerable<AssetDto>> GetByEmployeeAsync(int employeeId)
    {
        var assets = await _repository.GetByEmployeeAsync(employeeId);
        return _mapper.Map<IEnumerable<AssetDto>>(assets);
    }

    public async Task<IEnumerable<AssetDto>> GetMyAssetsAsync()
    {
        var employeeId = _currentUserService.EmployeeId;

        var assets = await _repository.GetByEmployeeAsync(employeeId);

        return _mapper.Map<IEnumerable<AssetDto>>(assets);
    }
}