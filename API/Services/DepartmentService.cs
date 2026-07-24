using API.DTOs.Department;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Entities;
using AutoMapper;

namespace API.Services;

public class DepartmentService(
    IDepartmentRepository repository,
    IMapper mapper) : IDepartmentService
{
    private readonly IDepartmentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department == null)
            return null;

        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        if (await _repository.ExistsByNameAsync(dto.Name))
            throw new Exception("Department already exists.");

        var department = _mapper.Map<Department>(dto);

        await _repository.AddAsync(department);

        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department == null)
            return false;

        _mapper.Map(dto, department);

        await _repository.UpdateAsync(department);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department == null)
            return false;

        await _repository.DeleteAsync(department);

        return true;
    }
}