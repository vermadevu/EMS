export interface DesignationListState {
    pageNumber: number;
    pageSize: number;
    search: string;
    sortBy: string;
    sortDirection: 'asc' | 'desc';
}