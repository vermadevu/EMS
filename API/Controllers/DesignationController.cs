using API.Authorization;
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
    [HasPermission(Permissions.Designations.Read)]
    public async Task<ActionResult<IEnumerable<DesignationDto>>> GetAll()
    {
        var designations = await _designationService.GetAllAsync();


        return Ok(designations);
    }


    [HttpGet("{id:int}")]
    [HasPermission(Permissions.Designations.Read)]
    public async Task<ActionResult<DesignationDto>> GetById(int id)
    {
        var designation = await _designationService.GetByIdAsync(id);

        if (designation == null)
            return NotFound();

        return designation;
    }


    [HttpPost]
    [HasPermission(Permissions.Designations.Create)]
    public async Task<ActionResult<DesignationDto>> Create(CreateDesignationDto dto)
    {
        var designation = await _designationService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = designation.Id },
            designation);
    }


    [HttpPut("{id:int}")]
    [HasPermission(Permissions.Designations.Update)]
    public async Task<ActionResult> Update(int id, UpdateDesignationDto dto)
    {
        var updated = await _designationService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [HasPermission(Permissions.Designations.Delete)]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _designationService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}