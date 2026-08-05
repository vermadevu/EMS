using API.DTOs.User;
using API.Helpers.Pagination;

namespace API.Interfaces.Repository
{
    public interface IApplicationUserRepository
    {
        Task<PagedResult<UserListItemDto>> GetPagedAsync(UserQueryParams query);
    }
}
