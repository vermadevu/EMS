export interface CurrentUser {
  id: string;
  employeeId: number;
  displayName: string;
  email: string;
  employeeStatus: number;
  roles: string[];
  permissions: string[];
}