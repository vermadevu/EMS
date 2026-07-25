using API.Models.Identity;

namespace API.Models.Authorization;

public class UserPermission
{
    public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
    public int PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
    public bool IsAllowed { get; set; }
}