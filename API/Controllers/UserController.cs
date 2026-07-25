using API.DTOs.User;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserService userService) : BaseApiController
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        return Ok(await _userService.GetAllAsync());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<UserDto>> GetUser(string id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("roles")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<IEnumerable<string>>> GetRoles()
    {
        return Ok(await _userService.GetRolesAsync());
    }

    [HttpGet("{id}/roles")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string id)
    {
        var roles = await _userService.GetUserRolesAsync(id);

        if (roles == null)
            return NotFound();

        return Ok(roles);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        var user = await _userService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id },
            user);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDto dto)
    {
        var updated = await _userService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{id}/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserRoles(string id, UpdateUserRolesDto dto)
    {
        var updated = await _userService.UpdateRolesAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id}/activate")]
    [Authorize(Roles = "Admin, HR")]
    public async Task<IActionResult> ActivateUser(string id)
    {
        var activated = await _userService.ActivateAsync(id);

        if (!activated)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id}/deactivate")]
    [Authorize(Roles = "Admin, HR")]
    public async Task<IActionResult> DeactivateUser(string id)
    {
        var deactivated = await _userService.DeactivateAsync(id);

        if (!deactivated)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/reset-password")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ResetPassword(string id, ResetPasswordDto dto)
    {
        var reset = await _userService.ResetPasswordAsync(id, dto);

        if (!reset)
            return NotFound();

        return NoContent();
    }
}