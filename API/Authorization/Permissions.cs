namespace API.Authorization;

public static class Permissions
{
    public static class Dashboard
    {
        public const string View = "Dashboard.View";
    }

    public static class Employees
    {
        public const string Read = "Employee.Read";
        public const string Create = "Employee.Create";
        public const string Update = "Employee.Update";
        public const string Delete = "Employee.Delete";
        public const string Activate = "Employee.Activate";
        public const string CompleteOnboarding = "Employee.CompleteOnboarding";
    }

    public static class Departments
    {
        public const string Read = "Department.Read";
        public const string Create = "Department.Create";
        public const string Update = "Department.Update";
        public const string Delete = "Department.Delete";
    }

    public static class Designations
    {
        public const string Read = "Designation.Read";
        public const string Create = "Designation.Create";
        public const string Update = "Designation.Update";
        public const string Delete = "Designation.Delete";
    }

    public static class Users
    {
        public const string Read = "User.Read";
        public const string Create = "User.Create";
        public const string Update = "User.Update";
        public const string UpdateRoles = "User.UpdateRoles";
        public const string Activate = "User.Activate";
        public const string Deactivate = "User.Deactivate";
        public const string ResetPassword = "User.ResetPassword";
    }

    public static class Assets
    {
        public const string Read = "Asset.Read";
        public const string Create = "Asset.Create";
        public const string Update = "Asset.Update";
        public const string Delete = "Asset.Delete";
        public const string Assign = "Asset.Assign";
        public const string Return = "Asset.Return";
    }

    public static class Documents
    {
        public const string Read = "Document.Read";
        public const string Upload = "Document.Upload";
        public const string Delete = "Document.Delete";

        public const string ReadOwn = "Document.ReadOwn";
        public const string UploadOwn = "Document.UploadOwn";
        public const string DeleteOwn = "Document.DeleteOwn";
    }
}