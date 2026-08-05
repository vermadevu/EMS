export interface UserListState {
    pageNumber: number;
    pageSize: number;
    search: string;
    role?: string;
    isActive?: boolean;
}