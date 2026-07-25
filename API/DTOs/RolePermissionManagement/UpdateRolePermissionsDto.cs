using System.ComponentModel.DataAnnotations;

namespace API.DTOs.RolePermissionManagement;

public class UpdateRolePermissionsDto
{
    [Required]
    public List<int> PermissionIds { get; set; } = [];
}