namespace API.DTOs.Permission;

public class PermissionCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public int TotalPermissions { get; set; }
    public int AssignedPermissions { get; set; }

    public List<PermissionAssignmentDto> Permissions { get; set; } = [];
}