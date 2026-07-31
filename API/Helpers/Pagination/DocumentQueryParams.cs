namespace API.Helpers.Pagination;

public class DocumentQueryParams : PaginationParams
{
    public string? Search { get; set; }
    public string SortBy { get; set; } = "FullName";
    public string SortDirection { get; set; } = "asc";
}