using API.DTOs.Asset;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AssetController(IAssetService service) : BaseApiController
{
    private readonly IAssetService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssets()
    {
        var assets = await _service.GetAllAsync();
        return Ok(assets);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AssetDto>> GetAsset(int id)
    {
        var asset = await _service.GetByIdAsync(id);

        if (asset == null)
            return NotFound();

        return Ok(asset);
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<ActionResult<AssetDto>> CreateAsset(CreateAssetDto dto)
    {
        var asset = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetAsset),
            new { id = asset.Id },
            asset);
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsset(int id, UpdateAssetDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPost("{id:int}/assign")]
    public async Task<IActionResult> AssignAsset(int id, AssignAssetDto dto)
    {
        var assigned = await _service.AssignAssetAsync(id, dto);

        if (!assigned)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Admin,HR")]
    [HttpPost("{id:int}/return")]
    public async Task<IActionResult> ReturnAsset(int id)
    {
        var returned = await _service.ReturnAssetAsync(id);

        if (!returned)
            return NotFound();

        return NoContent();
    }
}