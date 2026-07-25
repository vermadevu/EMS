export interface LoginResponse {
  token: string;
  email: string;
  userName: string;
  employeeStatus: number;
  roles: string[];
}