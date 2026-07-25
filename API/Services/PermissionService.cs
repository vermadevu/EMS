using API.DTOs.Permission;
using API.Interfaces.Repository;
using API.Interfaces.Service;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services
{
    public class PermissionService(
        IPermissionRepository repository,
        IMemoryCache cache,
        IMapper mapper) : IPermissionService
    {

        private readonly IPermissionRepository _repository = repository;
        private readonly IMemoryCache _cache = cache;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            var permissions = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }

        public async Task<HashSet<string>> GetPermissionsAsync(string userId)
        {
            if (_cache.TryGetValue(CacheKey(userId), out HashSet<string>? permissions))
            {
                return permissions!;
            }

            permissions = await _repository.GetEffectivePermissionsAsync(userId);

            _cache.Set(
                CacheKey(userId),
                permissions,
                TimeSpan.FromMinutes(10));

            return permissions;
        }

        public async Task<bool> HasPermissionAsync( string userId, string permission)
        {
            var permissions = await GetPermissionsAsync(userId);

            return permissions.Contains(permission);
        }

        public Task RefreshPermissionsAsync(string userId)
        {
            _cache.Remove(CacheKey(userId));

            return Task.CompletedTask;
        }

        private static string CacheKey(string userId)
        {
            return $"permissions:{userId}";
        }
    }
}
