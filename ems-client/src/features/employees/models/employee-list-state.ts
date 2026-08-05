export interface EmployeeListState {
    pageNumber: number;
    pageSize: number;
    search: string;
    departmentId?: number;
    designationId?: number;
    status?: string | string[];
    sortBy: string;
    sortDirection: 'asc' | 'desc';
}