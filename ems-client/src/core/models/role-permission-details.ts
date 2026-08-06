import { PermissionCategory } from './permission-category';

export interface RolePermissionDetails {
    roleId: string;
    roleName: string;
    categories: PermissionCategory[];
}