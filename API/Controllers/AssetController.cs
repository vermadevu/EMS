using API.Authorization;
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
    [HasPermission(Permissions.Assets.Read)]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssets()
    {
        var assets = await _service.GetAllAsync();
        return Ok(assets);
    }

    [HttpGet("{id:int}")]
    [HasPermission(Permissions.Assets.Read)]
    public async Task<ActionResult<AssetDto>> GetAsset(int id)
    {
        var asset = await _service.GetByIdAsync(id);

        if (asset == null)
            return NotFound();

        return Ok(asset);
    }

    [HttpPost]
    [HasPermission(Permissions.Assets.Create)]
    public async Task<ActionResult<AssetDto>> CreateAsset(CreateAssetDto dto)
    {
        var asset = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetAsset),
            new { id = asset.Id },
            asset);
    }

    [HttpPut("{id:int}")]
    [HasPermission(Permissions.Assets.Update)]
    public async Task<IActionResult> UpdateAsset(int id, UpdateAssetDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [HasPermission(Permissions.Assets.Delete)]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id:int}/assign")]
    [HasPermission(Permissions.Assets.Assign)]
    public async Task<IActionResult> AssignAsset(int id, AssignAssetDto dto)
    {
        var assigned = await _service.AssignAssetAsync(id, dto);

        if (!assigned)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id:int}/return")]
    [HasPermission(Permissions.Assets.Return)]
    public async Task<IActionResult> ReturnAsset(int id)
    {
        var returned = await _service.ReturnAssetAsync(id);

        if (!returned)
            return NotFound();

        return NoContent();
    }
}