using API.Data;
using API.DTOs.User;
using API.Helpers.Pagination;
using API.Interfaces.Repository;
using API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ApplicationUserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;
        public async Task<PagedResult<UserListItemDto>> GetPagedAsync(UserQueryParams query)
        {
            var users = _context.Users
                .Include(u => u.Employee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();

                users = users.Where(u =>
                    u.DisplayName.ToLower().Contains(search) ||
                    u.Employee.EmployeeCode.ToLower().Contains(search) ||
                    u.UserName!.ToLower().Contains(search));
            }

            if (query.IsActive.HasValue)
            {
                users = users.Where(u => u.IsActive == query.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                var userIds = await (
                    from userRole in _context.UserRoles
                    join role in _context.Roles
                        on userRole.RoleId equals role.Id
                    where role.Name == query.Role
                    select userRole.UserId
                ).ToListAsync();

                users = users.Where(u => userIds.Contains(u.Id));
            }

            var totalCount = await users.CountAsync();

            var items = await users
                .OrderBy(u => u.DisplayName)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var dtos = new List<UserListItemDto>();

            foreach (var user in items)
            {
                dtos.Add(new UserListItemDto
                {
                    Id = user.Id,
                    EmployeeId = user.EmployeeId,
                    EmployeeCode = user.Employee.EmployeeCode,
                    FullName = user.DisplayName,
                    Username = user.UserName!,
                    ProfileImage = user.Employee.ProfileImage,
                    IsActive = user.IsActive,
                    Roles = (await _userManager.GetRolesAsync(user)).ToList()
                });
            }

            return new PagedResult<UserListItemDto>
            {
                Items = dtos,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
