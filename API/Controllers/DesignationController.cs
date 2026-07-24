using API.DTOs.Designation;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DesignationController(IDesignationService designationService) : BaseApiController
{
    private readonly IDesignationService _designationService = designationService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DesignationDto>>> GetAll()
    {
        var designations = await _designationService.GetAllAsync();


        return Ok(designations);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<DesignationDto>> GetById(int id)
    {
        var designation = await _designationService.GetByIdAsync(id);

        if (designation == null)
            return NotFound();

        return designation;
    }


    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<ActionResult<DesignationDto>> Create(CreateDesignationDto dto)
    {
        var designation = await _designationService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = designation.Id },
            designation);
    }


    [Authorize(Roles = "Admin,HR")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateDesignationDto dto)
    {
        var updated = await _designationService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _designationService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}