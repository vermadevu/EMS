using API.DTOs.Employee;
using API.Exceptions;
using API.Helpers.Pagination;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using API.Models.Entities;
using API.Models.Enums;
using API.Repositories;
using AutoMapper;

namespace API.Services;

public class EmployeeService(
    IEmployeeRepository repository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IEmployeeService
{
    private readonly IEmployeeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            return null;

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        if (await _repository.ExistsByEmailAsync(dto.Email))
            throw new BadRequestException("Email already exists.");

        if (!await _repository.DepartmentExistsAsync(dto.DepartmentId))
            throw new NotFoundException("Department not found.");

        if (!await _repository.DesignationExistsAsync(dto.DesignationId))
            throw new NotFoundException("Designation not found.");

        if (dto.ManagerId.HasValue &&
            !await _repository.ManagerExistsAsync(dto.ManagerId.Value))
            throw new NotFoundException("Manager not found.");

        var employee = _mapper.Map<Employee>(dto);

        employee.EmployeeCode = await GenerateEmployeeCodeAsync();

        employee.Status = EmployeeStatus.Pending;

        await _repository.AddAsync(employee);

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        if (!await _repository.DepartmentExistsAsync(dto.DepartmentId))
            throw new NotFoundException("Department not found.");

        if (!await _repository.DesignationExistsAsync(dto.DesignationId))
            throw new NotFoundException("Designation not found.");

        if (dto.ManagerId.HasValue)
        {
            if (dto.ManagerId.Value == id)
                throw new BadRequestException("An employee cannot be their own manager.");


            if (!await _repository.ManagerExistsAsync(dto.ManagerId.Value))
                throw new NotFoundException("Manager not found.");
        }

        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            return false;
        // Reject Update if the email already exists with other employee
        if (await _repository.ExistsByEmailAsync(dto.Email, id))
            throw new BadRequestException("Email already exists.");

        _mapper.Map(dto, employee);

        await _repository.UpdateAsync(employee);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            return false;

        await _repository.DeleteAsync(employee);

        return true;
    }

    private async Task<string> GenerateEmployeeCodeAsync()
    {
        var lastCode = await _repository.GetLastEmployeeCodeAsync();

        if (string.IsNullOrWhiteSpace(lastCode))
            return "E0001";

        var number = int.Parse(lastCode.Substring(3));

        return $"E{(number + 1):D4}";
    }

    public async Task<bool> CompleteOnboardingAsync()
    {
        var employeeId = _currentUserService.EmployeeId;

        var employee = await _repository.GetByIdAsync(employeeId);

        if (employee == null)
        {
            return false;
        }

        employee.Status = EmployeeStatus.DocumentsSubmitted;

        await _repository.UpdateAsync(employee);

        return true;
    }

    public async Task<bool> ActivateEmployeeAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
        {
            return false;
        }

        if (employee.Status != EmployeeStatus.DocumentsSubmitted)
        {
            throw new BadRequestException("Employee has not submitted documents.");
        }

        employee.Status = EmployeeStatus.Active;

        await _repository.UpdateAsync(employee);

        return true;
    }

    public async Task<PagedResult<EmployeeListItemDto>> GetPagedAsync(EmployeeQueryParams queryParams)
    {
        var result = await _repository.GetPagedAsync(queryParams);

        return new PagedResult<EmployeeListItemDto>
        {
            Items = _mapper.Map<List<EmployeeListItemDto>>(result.Items),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
}