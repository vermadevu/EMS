export interface DocumentListState {
    pageNumber: number;
    pageSize: number;
    search: string;
    sortBy: string;
    sortDirection: 'asc' | 'desc';
}