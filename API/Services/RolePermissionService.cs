using API.DTOs.RolePermissionManagement;
using API.Exceptions;
using API.Interfaces.Repository;
using API.Interfaces.Service;

namespace API.Services
{
    public class RolePermissionService(IRolePermissionRepository repository, IPermissionService permissionService) : IRolePermissionService
    {
        private readonly IRolePermissionRepository _repository = repository;
        private readonly IPermissionService _permissionService = permissionService;
     
        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            var roles = await _repository.GetRolesAsync();

            return roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name!
            });
        }

        public async Task<RolePermissionsDto> GetRolePermissionsAsync(string roleId)
        {
            return await _repository.GetRolePermissionsAsync(roleId);
        }

        public async Task UpdateRolePermissionsAsync(string roleId, UpdateRolePermissionsDto dto)
        {
            if (dto.PermissionIds == null || dto.PermissionIds.Count == 0)
            {
                throw new BadRequestException("At least one permission must be selected.");
            }

            if (!await _repository.AreValidPermissionIdsAsync(dto.PermissionIds))
            {
                throw new BadRequestException("One or more permission IDs are invalid.");
            }

            await _repository.UpdateRolePermissionsAsync(roleId, dto.PermissionIds);

            var userIds = await _repository.GetUserIdsInRoleAsync(roleId);

            foreach (var userId in userIds)
            {
                await _permissionService.RefreshPermissionsAsync(userId);
            }
        }
    }
}   
