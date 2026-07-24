using API.Models.Identity;

namespace API.Interfaces.Service
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user, IList<string> roles);
    }
}
