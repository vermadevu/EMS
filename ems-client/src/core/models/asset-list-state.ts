export interface AssetListState {
    pageNumber: number;
    pageSize: number;
    search: string;
    assetType?: number;
    status?: number;
    sortBy: string;
    sortDirection: 'asc' | 'desc';
}