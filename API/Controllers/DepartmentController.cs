using API.Authorization;
using API.DTOs.Department;
using API.Helpers.Pagination;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DepartmentController(IDepartmentService departmentService) : BaseApiController
{
    private readonly IDepartmentService _departmentService = departmentService;

    [HttpGet("all")]
    [HasPermission(Permissions.Departments.Read)]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();


        return Ok(departments);
    }

    [HttpGet("{id:int}")]
    [HasPermission(Permissions.Departments.Read)]
    public async Task<ActionResult<DepartmentDto>> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);

        if (department == null)
            return NotFound();

        return department;
    }


    [HttpPost]
    [HasPermission(Permissions.Departments.Create)]
    public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto)
    {
        var department = await _departmentService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = department.Id },
            department);
    }


    [HttpPut("{id:int}")]
    [HasPermission(Permissions.Departments.Update)]
    public async Task<ActionResult> Update(int id, UpdateDepartmentDto dto)
    {
        var updated = await _departmentService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [HasPermission(Permissions.Departments.Delete)]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _departmentService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet]
    [HasPermission(Permissions.Departments.Read)]
    public async Task<ActionResult<PagedResult<DepartmentListItemDto>>> GetDepartments([FromQuery] DepartmentQueryParams queryParams)
    {
        return Ok(await _departmentService.GetPagedAsync(queryParams));
    }
}