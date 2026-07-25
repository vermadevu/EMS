using API.DTOs.Designation;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Entities;
using AutoMapper;

namespace API.Services;

public class DesignationService(
    IDesignationRepository repository,
    IMapper mapper) : IDesignationService
{
    private readonly IDesignationRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DesignationDto>> GetAllAsync()
    {
        var designations = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<DesignationDto>>(designations);
    }

    public async Task<DesignationDto?> GetByIdAsync(int id)
    {
        var designation = await _repository.GetByIdAsync(id);

        if (designation == null)
            return null;

        return _mapper.Map<DesignationDto>(designation);
    }

    public async Task<DesignationDto> CreateAsync(CreateDesignationDto dto)
    {
        if (await _repository.ExistsByNameAsync(dto.Name))
            throw new BadRequestException("Designation already exists.");

        var designation = _mapper.Map<Designation>(dto);

        await _repository.AddAsync(designation);

        return _mapper.Map<DesignationDto>(designation);
    }

    public async Task<bool> UpdateAsync(int id, UpdateDesignationDto dto)
    {
        var designation = await _repository.GetByIdAsync(id);

        if (designation == null)
            return false;

        _mapper.Map(dto, designation);

        await _repository.UpdateAsync(designation);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var designation = await _repository.GetByIdAsync(id);

        if (designation == null)
            return false;

        await _repository.DeleteAsync(designation);

        return true;
    }
}