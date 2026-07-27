export const PERMISSIONS = {

    Dashboard: {
        View: 'Dashboard.View'
    },

    Employees: {
        Read: 'Employee.Read',
        Create: 'Employee.Create',
        Update: 'Employee.Update',
        Delete: 'Employee.Delete',
        Activate: 'Employee.Activate',
        CompleteOnboarding: 'Employee.CompleteOnboarding'
    },

    Departments: {
        Read: 'Department.Read',
        Create: 'Department.Create',
        Update: 'Department.Update',
        Delete: 'Department.Delete'
    },

    Designations: {
        Read: 'Designation.Read',
        Create: 'Designation.Create',
        Update: 'Designation.Update',
        Delete: 'Designation.Delete'
    },

    Users: {
        Read: 'User.Read',
        Create: 'User.Create',
        Update: 'User.Update',
        UpdateRoles: 'User.UpdateRoles',
        Activate: 'User.Activate',
        Deactivate: 'User.Deactivate',
        ResetPassword: 'User.ResetPassword'
    },

    Assets: {
        Read: 'Asset.Read',
        Create: 'Asset.Create',
        Update: 'Asset.Update',
        Delete: 'Asset.Delete',
        Assign: 'Asset.Assign',
        Return: 'Asset.Return'
    },

    Documents: {
        Read: 'Document.Read',
        Upload: 'Document.Upload',
        Delete: 'Document.Delete',

        ReadOwn: 'Document.ReadOwn',
        UploadOwn: 'Document.UploadOwn',
        DeleteOwn: 'Document.DeleteOwn'
    }

} as const;