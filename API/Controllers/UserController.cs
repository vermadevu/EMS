using API.Authorization;
using API.DTOs.User;
using API.Helpers.Pagination;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserService userService) : BaseApiController
{
    private readonly IUserService _userService = userService;

    [HttpGet("all")]
    [HasPermission(Permissions.Users.Read)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        return Ok(await _userService.GetAllAsync());
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.Users.Read)]
    public async Task<ActionResult<UserDto>> GetUser(string id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("roles")]
    [HasPermission(Permissions.Users.Read)]
    public async Task<ActionResult<IEnumerable<string>>> GetRoles()
    {
        return Ok(await _userService.GetRolesAsync());
    }

    [HttpGet("{id}/roles")]
    [HasPermission(Permissions.Users.Read)]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string id)
    {
        var roles = await _userService.GetUserRolesAsync(id);

        if (roles == null)
            return NotFound();

        return Ok(roles);
    }

    [HttpPost]
    [HasPermission(Permissions.Users.Create)]
    public async Task<ActionResult<CreateUserResponseDto>> CreateUser(CreateUserDto dto)
    {
        return Ok(await _userService.CreateAsync(dto));

    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.Users.Update)]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDto dto)
    {
        var updated = await _userService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{id}/roles")]
    [HasPermission(Permissions.Users.UpdateRoles)]
    public async Task<IActionResult> UpdateUserRoles(string id, UpdateUserRolesDto dto)
    {
        var updated = await _userService.UpdateRolesAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id}/activate")]
    [HasPermission(Permissions.Users.Activate)]
    public async Task<IActionResult> ActivateUser(string id)
    {
        var activated = await _userService.ActivateAsync(id);

        if (!activated)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id}/deactivate")]
    [HasPermission(Permissions.Users.Deactivate)]
    public async Task<IActionResult> DeactivateUser(string id)
    {
        var deactivated = await _userService.DeactivateAsync(id);

        if (!deactivated)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/reset-password")]
    [HasPermission(Permissions.Users.ResetPassword)]
    public async Task<IActionResult> ResetPassword(string id, ResetPasswordDto dto)
    {
        var reset = await _userService.ResetPasswordAsync(id, dto);

        if (!reset)
            return NotFound();

        return NoContent();
    }

    [HttpGet("available-employees")]
    [HasPermission(Permissions.Users.Read)]
    public async Task<ActionResult<List<AvailableEmployeeDto>>> GetAvailableEmployees()
    {
        return Ok(
            await _userService.GetAvailableEmployeesAsync()
        );
    }

    [HttpGet]
    [HasPermission(Permissions.Users.Read)]
    public async Task<ActionResult<PagedResult<UserListItemDto>>> GetUsers([FromQuery] UserQueryParams query)
    {
        return Ok(await _userService.GetPagedAsync(query));
    }

}