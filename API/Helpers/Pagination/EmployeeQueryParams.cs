using API.Models.Enums;

namespace API.Helpers.Pagination
{
    public class EmployeeQueryParams : PaginationParams
    {
        public string? Search { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public List<EmployeeStatus>? Status { get; set; }
        public string SortBy { get; set; } = "JoiningDate";
        public string SortDirection { get; set; } = "desc";
    }
}
