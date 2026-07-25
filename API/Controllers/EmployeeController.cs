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
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var employee = await _employeeService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById),
            new { id = employee.Id },
            employee);
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateEmployeeDto dto)
    {
        var updated = await _employeeService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _employeeService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("me/complete-onboarding")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> CompleteOnboarding()
    {
        var result = await _employeeService.CompleteOnboardingAsync();

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id:int}/activate")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> ActivateEmployee(int id)
    {
        var result = await _employeeService.ActivateEmployeeAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }

}