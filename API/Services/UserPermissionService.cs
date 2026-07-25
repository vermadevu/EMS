using API.DTOs.UserPermissionManagement;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Interfaces.Service;

namespace API.Services
{
    public class UserPermissionService(IUserPermissionRepository repository, IPermissionService permissionService) : IUserPermissionService
    {
        private readonly IPermissionService _permissionService = permissionService;
        private readonly IUserPermissionRepository _repository = repository;

        public async Task<IEnumerable<UserListDto>> GetUsersAsync()
        {
            return await _repository.GetUsersAsync();
        }
        public async Task<UserPermissionsDto> GetUserPermissionsAsync(string userId)
        {
            return await _repository.GetUserPermissionsAsync(userId);
        }

        public async Task UpdateUserPermissionsAsync(string userId, UpdateUserPermissionsDto dto)
        {
            var permissionIds = dto.Permissions
                .Select(p => p.PermissionId)
                .Distinct()
                .ToList();

            if (!await _repository.AreValidPermissionIdsAsync(permissionIds))
            {
                throw new BadRequestException("One or more permission IDs are invalid.");
            }

            await _repository.UpdateUserPermissionsAsync(userId, dto.Permissions);

            await _permissionService.RefreshPermissionsAsync(userId);
        }
    }
}
