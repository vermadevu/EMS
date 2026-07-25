using API.Models.Base;

namespace API.Models.Authorization;

public class Permission : BaseEntity
{
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Category { get; set; } = "";
    public string? Description { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
    public ICollection<UserPermission> UserPermissions { get; set; } = [];
}