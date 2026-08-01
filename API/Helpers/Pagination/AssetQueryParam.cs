using API.Models.Enums;

namespace API.Helpers.Pagination;

public class AssetQueryParams : PaginationParams
{
    public string? Search { get; set; }
    public AssetStatus? Status { get; set; }
    public AssetType? AssetType { get; set; }
    public string SortBy { get; set; } = "assetName";
    public string SortDirection { get; set; } = "asc";
}