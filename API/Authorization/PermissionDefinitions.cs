using API.Authorization;

namespace API.Authorization;

public static class PermissionDefinitions
{
    public static IEnumerable<PermissionDefinition> GetAll()
    {
        return
        [
            // ================= Dashboard =================

            new()
            {
                Name = Permissions.Dashboard.View,
                DisplayName = "View Dashboard",
                Category = "Dashboard",
                Description = "Allows viewing dashboard."
            },

            // ================= Employees =================

            new()
            {
                Name = Permissions.Employees.Read,
                DisplayName = "View Employees",
                Category = "Employee",
                Description = "Allows viewing employees."
            },
            new()
            {
                Name = Permissions.Employees.Profile,
                DisplayName = "Employee Profile",
                Category = "Employee",
                Description = "Allows manage employee profile."
            },

            new()
            {
                Name = Permissions.Employees.Create,
                DisplayName = "Create Employee",
                Category = "Employee",
                Description = "Allows creating employees."
            },

            new()
            {
                Name = Permissions.Employees.Update,
                DisplayName = "Update Employee",
                Category = "Employee",
                Description = "Allows updating employees."
            },

            new()
            {
                Name = Permissions.Employees.Delete,
                DisplayName = "Delete Employee",
                Category = "Employee",
                Description = "Allows deleting employees."
            },

            new()
            {
                Name = Permissions.Employees.Activate,
                DisplayName = "Activate Employee",
                Category = "Employee",
                Description = "Allows activating employees."
            },

            new()
            {
                Name = Permissions.Employees.Deactivate,
                DisplayName = "Dectivate Employee",
                Category = "Employee",
                Description = "Allows deactivating employees."
            },

            new()
            {
                Name = Permissions.Employees.CompleteOnboarding,
                DisplayName = "Complete Onboarding",
                Category = "Employee",
                Description = "Allows completing onboarding."
            },

            // ================= Departments =================

            new()
            {
                Name = Permissions.Departments.Read,
                DisplayName = "View Departments",
                Category = "Department",
                Description = "Allows viewing departments."
            },

            new()
            {
                Name = Permissions.Departments.Create,
                DisplayName = "Create Department",
                Category = "Department",
                Description = "Allows creating departments."
            },

            new()
            {
                Name = Permissions.Departments.Update,
                DisplayName = "Update Department",
                Category = "Department",
                Description = "Allows updating departments."
            },

            new()
            {
                Name = Permissions.Departments.Delete,
                DisplayName = "Delete Department",
                Category = "Department",
                Description = "Allows deleting departments."
            },

            // ================= Designations =================

            new()
            {
                Name = Permissions.Designations.Read,
                DisplayName = "View Designations",
                Category = "Designation",
                Description = "Allows viewing designations."
            },

            new()
            {
                Name = Permissions.Designations.Create,
                DisplayName = "Create Designation",
                Category = "Designation",
                Description = "Allows creating designations."
            },

            new()
            {
                Name = Permissions.Designations.Update,
                DisplayName = "Update Designation",
                Category = "Designation",
                Description = "Allows updating designations."
            },

            new()
            {
                Name = Permissions.Designations.Delete,
                DisplayName = "Delete Designation",
                Category = "Designation",
                Description = "Allows deleting designations."
            },

            // ================= Users =================

            new()
            {
                Name = Permissions.Users.Read,
                DisplayName = "View Users",
                Category = "User",
                Description = "Allows viewing users."
            },

            new()
            {
                Name = Permissions.Users.Create,
                DisplayName = "Create User",
                Category = "User",
                Description = "Allows creating users."
            },

            new()
            {
                Name = Permissions.Users.Update,
                DisplayName = "Update User",
                Category = "User",
                Description = "Allows updating users."
            },

            new()
            {
                Name = Permissions.Users.UpdateRoles,
                DisplayName = "Manage Role Permissions",
                Category = "User",
                Description = "Allows managing role permissions."
            },

            new()
            {
                Name = Permissions.Users.Activate,
                DisplayName = "Activate User",
                Category = "User",
                Description = "Allows activating users."
            },

            new()
            {
                Name = Permissions.Users.Deactivate,
                DisplayName = "Deactivate User",
                Category = "User",
                Description = "Allows deactivating users."
            },

            new()
            {
                Name = Permissions.Users.ResetPassword,
                DisplayName = "Reset Password",
                Category = "User",
                Description = "Allows resetting passwords."
            },

            // ================= Assets =================

            new()
            {
                Name = Permissions.Assets.Read,
                DisplayName = "View Assets",
                Category = "Asset",
                Description = "Allows viewing assets."
            },

             new()
            {
                Name = Permissions.Assets.ReadOwn,
                DisplayName = "View own Assets",
                Category = "Asset",
                Description = "Allows viewing own assets."
            },

            new()
            {
                Name = Permissions.Assets.Create,
                DisplayName = "Create Asset",
                Category = "Asset",
                Description = "Allows creating assets."
            },

            new()
            {
                Name = Permissions.Assets.Update,
                DisplayName = "Update Asset",
                Category = "Asset",
                Description = "Allows updating assets."
            },

            new()
            {
                Name = Permissions.Assets.Delete,
                DisplayName = "Delete Asset",
                Category = "Asset",
                Description = "Allows deleting assets."
            },

            new()
            {
                Name = Permissions.Assets.Assign,
                DisplayName = "Assign Asset",
                Category = "Asset",
                Description = "Allows assigning assets."
            },

            new()
            {
                Name = Permissions.Assets.Return,
                DisplayName = "Return Asset",
                Category = "Asset",
                Description = "Allows returning assets."
            },

            // ================= Documents =================

            new()
            {
                Name = Permissions.Documents.Read,
                DisplayName = "View Documents",
                Category = "Document",
                Description = "Allows viewing documents."
            },

            new()
            {
                Name = Permissions.Documents.Upload,
                DisplayName = "Upload Documents",
                Category = "Document",
                Description = "Allows uploading documents."
            },

            new()
            {
                Name = Permissions.Documents.Delete,
                DisplayName = "Delete Documents",
                Category = "Document",
                Description = "Allows deleting documents."
            },

            new()
            {
                Name = Permissions.Documents.ReadOwn,
                DisplayName = "View Own Documents",
                Category = "Document",
                Description = "Allows viewing own documents."
            },

            new()
            {
                Name = Permissions.Documents.UploadOwn,
                DisplayName = "Upload Own Documents",
                Category = "Document",
                Description = "Allows uploading own documents."
            },

            new()
            {
                Name = Permissions.Documents.DeleteOwn,
                DisplayName = "Delete Own Documents",
                Category = "Document",
                Description = "Allows deleting own documents."
            }
        ];
    }
}