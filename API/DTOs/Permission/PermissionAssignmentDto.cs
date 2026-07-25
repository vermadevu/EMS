namespace API.DTOs.Permission;

public class PermissionAssignmentDto
{
    public int PermissionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Assigned { get; set; }
}