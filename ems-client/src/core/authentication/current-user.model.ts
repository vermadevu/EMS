export interface CurrentUser {
  id: string;
  employeeId: number;
  displayName: string;
  email: string;
  employeeStatus: string;
  roles: string[];
  permissions: string[];
  profileImage: string;
}