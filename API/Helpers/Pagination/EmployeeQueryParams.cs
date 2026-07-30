using API.Models.Enums;

namespace API.Helpers.Pagination
{
    public class EmployeeQueryParams
    {
        private const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize
                ? MaxPageSize
                : value;
        }
        public string? Search { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public EmployeeStatus? Status { get; set; }
        public string SortBy { get; set; } = "JoiningDate";
        public string SortDirection { get; set; } = "desc";
    }
}
