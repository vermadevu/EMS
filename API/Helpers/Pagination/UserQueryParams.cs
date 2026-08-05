namespace API.Helpers.Pagination
{
    public class UserQueryParams : PaginationParams
    {
        public string? Search { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
    }
}
