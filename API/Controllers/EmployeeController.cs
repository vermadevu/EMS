using API.Authorization;
using API.DTOs;
using API.DTOs.Employee;
using API.Helpers;
using API.Helpers.Pagination;
using API.Interfaces.Service;
using API.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class EmployeeController(IEmployeeService employeeService, ICurrentUserService currentUserService) : BaseApiController
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpGet("all")]
    [HasPermission(Permissions.Employees.Read)]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }


    [HttpGet("{id:int}")]
    [HasPermission(Permissions.Employees.Read)]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPost]
    [HasPermission(Permissions.Employees.Create)]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto employeeDto)
    {
        var employee = await _employeeService.CreateAsync(employeeDto);

        return CreatedAtAction(nameof(GetById),
            new { id = employee.Id },
            employee);
    }

    [HttpPut("{id:int}")]
    [HasPermission(Permissions.Employees.Update)]
    public async Task<ActionResult> Update(int id, UpdateEmployeeDto employeeDto)
    {
        var updated = await _employeeService.UpdateAsync(id, employeeDto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [HasPermission(Permissions.Employees.Delete)]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _employeeService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("me/complete-onboarding")]
    [HasPermission(Permissions.Employees.CompleteOnboarding)]
    public async Task<IActionResult> CompleteOnboarding()
    {
        var result = await _employeeService.CompleteOnboardingAsync();

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id:int}/activate")]
    [HasPermission(Permissions.Employees.Activate)]
    public async Task<IActionResult> ActivateEmployee(int id)
    {
        var result = await _employeeService.ActivateEmployeeAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id:int}/deactivate")]
    [HasPermission(Permissions.Employees.Deactivate)]
    public async Task<IActionResult> DeactivateEmployee(int id)
    {
        var result = await _employeeService.DeactivateEmployeeAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet]
    [HasPermission(Permissions.Employees.Read)]
    public async Task<ActionResult<PagedResult<EmployeeListItemDto>>> Get([FromQuery] EmployeeQueryParams queryParams)
    {
        return Ok(await _employeeService.GetPagedAsync(queryParams));
    }

    
    [HttpGet("statuses")]
    [HasPermission(Permissions.Employees.Read)]
    public IActionResult GetStatuses()
    {
        var statuses = Enum.GetValues<EmployeeStatus>()
            .Select(status => new
            {
                Value = status.ToString(),
                Label = status.ToString().ToDisplayName()
            });

        return Ok(statuses);
    }

    [HttpGet("managers")]
    [HasPermission(Permissions.Employees.Read)]
    public async Task<ActionResult<IEnumerable<EmployeeListItemDto>>> GetManagers()
    {
        return Ok(await _employeeService.GetManagersAsync());
    }

    [HttpGet("me")]
    [HasPermission(Permissions.Employees.Profile)]
    public async Task<ActionResult<EmployeeProfileDto>> GetMyProfile()
    {
        var employeeId = _currentUserService.EmployeeId;

        return Ok(await _employeeService.GetMyProfileAsync(employeeId));
    }

    [HttpPut("me")]
    [HasPermission(Permissions.Employees.Profile)]
    public async Task<IActionResult> UpdateMyProfile(UpdateProfileDto dto)
    {
        var employeeId = _currentUserService.EmployeeId;

        await _employeeService.UpdateMyProfileAsync(employeeId, dto);

        return NoContent();
    }

}