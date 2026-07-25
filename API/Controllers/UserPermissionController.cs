using API.Authorization;
using API.DTOs.UserPermissionManagement;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UserPermissionController(IUserPermissionService service): BaseApiController
{
    private readonly IUserPermissionService _service = service;

    [HttpGet("users")]
    [HasPermission(Permissions.Users.UpdateRoles)]
    public async Task<ActionResult<IEnumerable<UserListDto>>> GetUsers()
    {
        return Ok(await _service.GetUsersAsync());
    }

    [HttpGet("{userId}")]
    [HasPermission(Permissions.Users.UpdateRoles)]
    public async Task<ActionResult<UserPermissionsDto>> GetUserPermissions(string userId)
    {
        return Ok(await _service.GetUserPermissionsAsync(userId));
    }

    [HttpPut("{userId}")]
    [HasPermission(Permissions.Users.UpdateRoles)]
    public async Task<IActionResult> UpdateUserPermissions(
        string userId,
        UpdateUserPermissionsDto dto)
    {
        await _service.UpdateUserPermissionsAsync(userId, dto);

        return NoContent();
    }
}