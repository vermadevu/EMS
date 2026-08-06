import { Permission } from "./permission";

export interface PermissionCategory {
    name: string;
    totalPermissions: number;
    assignedPermissions: number;
    permissions: Permission[];
}