namespace API.Authorization;

public static class RolePermissions
{
    public static readonly IEnumerable<string> Admin =
        PermissionDefinitions.GetAll()
            .Select(p => p.Name);

    public static readonly IEnumerable<string> HR =
    [
        Permissions.Dashboard.View,

        Permissions.Employees.Read,
        Permissions.Employees.Create,
        Permissions.Employees.Update,
        Permissions.Employees.Delete,
        Permissions.Employees.Activate,

        Permissions.Departments.Read,
        Permissions.Departments.Create,
        Permissions.Departments.Update,
        Permissions.Departments.Delete,

        Permissions.Designations.Read,
        Permissions.Designations.Create,
        Permissions.Designations.Update,
        Permissions.Designations.Delete,

        Permissions.Users.Read,
        Permissions.Users.Create,
        Permissions.Users.Update,
        Permissions.Users.UpdateRoles,
        Permissions.Users.Activate,
        Permissions.Users.Deactivate,
        Permissions.Users.ResetPassword,

        Permissions.Assets.Read,
        Permissions.Assets.Create,
        Permissions.Assets.Update,
        Permissions.Assets.Delete,
        Permissions.Assets.Assign,
        Permissions.Assets.Return,

        Permissions.Documents.Read,
        Permissions.Documents.Upload,
Permissions.Documents.Delete
    ];

    public static readonly IEnumerable<string> Manager =
    [
        Permissions.Dashboard.View,
        Permissions.Employees.Read,
        Permissions.Documents.Read,
        Permissions.Assets.Read

    ];

    public static readonly IEnumerable<string> Employee =
    [
        Permissions.Dashboard.View,
        Permissions.Documents.ReadOwn,
        Permissions.Documents.UploadOwn,
        Permissions.Documents.DeleteOwn,

        Permissions.Employees.CompleteOnboarding
    ];
}