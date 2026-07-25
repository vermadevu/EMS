using API.Models.Authorization;

namespace API.Interfaces.Repository
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<HashSet<string>> GetEffectivePermissionsAsync(string userId);
    }
}
