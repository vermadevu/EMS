using API.Authorization;
using API.DTOs.Asset;
using API.Helpers.Pagination;
using API.Interfaces.Service;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AssetController(IAssetService service) : BaseApiController
{
    private readonly IAssetService _service = service;

    [HttpGet]
    [HasPermission(Permissions.Assets.Read)]
    public async Task<ActionResult<PagedResult<AssetListItemDto>>> GetAssets(
        [FromQuery] AssetQueryParams queryParams)
    {
        return Ok(await _service.GetPagedAsync(queryParams));
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
    public async Task<ActionResult<AssetDto>> UpdateAsset(
        int id,
        UpdateAssetDto dto)
    {
        return Ok(await _service.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    [HasPermission(Permissions.Assets.Delete)]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }

    [HttpPatch("{id:int}/assign")]
    [HasPermission(Permissions.Assets.Assign)]
    public async Task<IActionResult> AssignAsset(
        int id,
        AssignAssetDto dto)
    {
        await _service.AssignAsync(id, dto);

        return NoContent();
    }

    [HttpPatch("{id:int}/return")]
    [HasPermission(Permissions.Assets.Return)]
    public async Task<IActionResult> ReturnAsset(int id)
    {
        await _service.ReturnAsync(id);

        return NoContent();
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetByEmployee(int employeeId)
    {
        return Ok(await _service.GetByEmployeeAsync(employeeId));
    }

    [HttpGet("me")]
    [HasPermission(Permissions.Assets.ReadOwn)]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetMyAssets()
    {
        return Ok(await _service.GetMyAssetsAsync());
    }
}