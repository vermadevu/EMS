using Microsoft.AspNetCore.Identity;

namespace API.Models.Authorization;

public class RolePermission
{
    public string RoleId { get; set; } = "";
    public IdentityRole Role { get; set; } = null!;
    public int PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
}