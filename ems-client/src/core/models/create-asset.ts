export interface CreateAsset {
    assetName: string;
    assetType: string;
    brand?: string;
    model?: string;
    serialNumber?: string;
    purchaseDate: string;
}