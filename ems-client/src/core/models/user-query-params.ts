export interface UserQueryParams {
  search?: string;
  role?: string;
  isActive?: boolean;
  pageNumber: number;
  pageSize: number;
}