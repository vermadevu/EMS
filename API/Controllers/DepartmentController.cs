using API.DTOs.Department;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DepartmentController(IDepartmentService departmentService) : BaseApiController
{
    private readonly IDepartmentService _departmentService = departmentService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();


        return Ok(departments);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartmentDto>> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);

        if (department == null)
            return NotFound();

        return department;
    }


    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto)
    {
        var department = await _departmentService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = department.Id },
            department);
    }


    [Authorize(Roles = "Admin,HR")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateDepartmentDto dto)
    {
        var updated = await _departmentService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _departmentService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}