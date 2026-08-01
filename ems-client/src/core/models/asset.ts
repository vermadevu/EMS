export interface Asset {
    id: number;
    assetCode: string;
    assetName: string;
    assetType: string;
    brand?: string;
    model?: string;
    serialNumber?: string;
    purchaseDate: string;
    status: string;
    employeeId?: number;
    employeeName?: string;
    employeeCode?: string;
}