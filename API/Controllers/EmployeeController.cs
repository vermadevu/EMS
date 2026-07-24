using API.DTOs.Employee;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class EmployeeController(IEmployeeService service) : BaseApiController
{
    private readonly IEmployeeService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
    {
        var employees = await _service.GetAllAsync();
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var employee = await _service.GetByIdAsync(id);

        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var employee = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById),
            new { id = employee.Id },
            employee);
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateEmployeeDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}