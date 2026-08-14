export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  email: string;
  userName: string;
  employeeStatus: number;
  roles: string[];
}