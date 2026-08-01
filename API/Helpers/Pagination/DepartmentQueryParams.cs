namespace API.Helpers.Pagination
{
    public class DepartmentQueryParams : PaginationParams
    {
        public string? Search { get; set; }
        public string SortBy { get; set; } = "Name";
        public string SortDirection { get; set; } = "asc";
    }
}
