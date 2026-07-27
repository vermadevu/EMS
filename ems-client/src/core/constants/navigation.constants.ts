import { NavigationItem } from "../layouts/shell/models/navigation-item.model";
import { APP_ROUTES } from "./app-routes";
import { PERMISSIONS } from "./permission";

export const NAVIGATION_ITEMS: NavigationItem[] = [

    {
        label: 'Dashboard',
        route: APP_ROUTES.Dashboard,
        icon: 'dashboard',
        permission: PERMISSIONS.Dashboard.View
    },

    {
        label: 'Employees',
        route: APP_ROUTES.Employees,
        icon: 'badge',
        permission: PERMISSIONS.Employees.Read
    },

    {
        label: 'Departments',
        route: APP_ROUTES.Departments,
        icon: 'apartment',
        permission: PERMISSIONS.Departments.Read
    },

    {
        label: 'Designations',
        route: APP_ROUTES.Designations,
        icon: 'work',
        permission: PERMISSIONS.Designations.Read
    },

    {
        label: 'Assets',
        route: APP_ROUTES.Assets,
        icon: 'inventory_2',
        permission: PERMISSIONS.Assets.Read
    },

    {
        label: 'Documents',
        route: APP_ROUTES.Documents,
        icon: 'description',
        permission: PERMISSIONS.Documents.Read
    },

    {
        label: 'Users',
        route: APP_ROUTES.Users,
        icon: 'group',
        permission: PERMISSIONS.Users.Read
    },

    {
        label: 'Role Permissions',
        route: APP_ROUTES.RolePermissions,
        icon: 'admin_panel_settings',
        permission: PERMISSIONS.Users.UpdateRoles
    },

    {
        label: 'User Permissions',
        route: APP_ROUTES.UserPermissions,
        icon: 'security',
        permission: PERMISSIONS.Users.UpdateRoles
    }

];