using System.Security.Claims;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;

namespace API.Authorization;

public class PermissionAuthorizationHandler(IPermissionService permissionService) : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService = permissionService;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
            return;

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return;

        var hasPermission = await _permissionService.HasPermissionAsync(
            userId,
            requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}