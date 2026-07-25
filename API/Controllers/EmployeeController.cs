using API.Authorization;
using API.DTOs.Employee;
using API.Interfaces.Service;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class EmployeeController(IEmployeeService employeeService) : BaseApiController
{
    private readonly IEmployeeService _employeeService = employeeService;

    [HttpGet]
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
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var employee = await _employeeService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById),
            new { id = employee.Id },
            employee);
    }

    [HttpPut("{id:int}")]
    [HasPermission(Permissions.Employees.Update)]
    public async Task<ActionResult> Update(int id, UpdateEmployeeDto dto)
    {
        var updated = await _employeeService.UpdateAsync(id, dto);

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

}