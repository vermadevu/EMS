export interface UserListItem {
  id: string;
  employeeId: number;
  employeeCode: string;
  fullName: string;
  username: string;
  profileImage: string | null;
  isActive: boolean;
  roles: string[];
}