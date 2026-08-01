export interface DepartmentListState {
  pageNumber: number;
  pageSize: number;
  search: string;
  sortBy: string;
  sortDirection: 'asc' | 'desc';
}