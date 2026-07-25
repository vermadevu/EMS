using API.Authorization;
using API.DTOs.RolePermissionManagement;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RolePermissionController(IRolePermissionService service) : BaseApiController
    {
        private readonly IRolePermissionService _service = service;

        [HttpGet("roles")]
        [HasPermission(Permissions.Users.UpdateRoles)]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            return Ok(await _service.GetRolesAsync());
        }

        [HttpGet("{roleId}")]
        [HasPermission(Permissions.Users.UpdateRoles)]
        public async Task<ActionResult<RolePermissionsDto>> GetRolePermissions(string roleId)
        {
            return Ok(await _service.GetRolePermissionsAsync(roleId));
        }

        [HttpPut("{roleId}")]
        [HasPermission(Permissions.Users.UpdateRoles)]
        public async Task<IActionResult> UpdateRolePermissions(string roleId, UpdateRolePermissionsDto dto)
        {
            await _service.UpdateRolePermissionsAsync(roleId, dto);

            return NoContent();
        }
    }
}
