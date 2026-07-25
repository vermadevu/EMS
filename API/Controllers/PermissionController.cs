using API.Authorization;
using API.DTOs.Permission;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class PermissionController(IPermissionService permissionService)
    : BaseApiController
{
    private readonly IPermissionService _permissionService = permissionService;

    [HasPermission(Permissions.Users.UpdateRoles)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAll()
    {
        return Ok(await _permissionService.GetAllAsync());
    }
}