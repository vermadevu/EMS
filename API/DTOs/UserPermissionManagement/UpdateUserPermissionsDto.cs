namespace API.DTOs.UserPermissionManagement
{
    public class UpdateUserPermissionsDto
    {
        public List<UserPermissionOverrideDto> Permissions { get; set; } = [];
        
    }
}
