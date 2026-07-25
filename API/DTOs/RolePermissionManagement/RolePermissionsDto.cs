using API.DTOs.Permission;

namespace API.DTOs.RolePermissionManagement
{
    public class RolePermissionsDto
    {
        public string RoleId { get; set; } = "";
        public string RoleName { get; set; } = "";
        public List<PermissionCategoryDto> Categories { get; set; } = [];
    }
}
