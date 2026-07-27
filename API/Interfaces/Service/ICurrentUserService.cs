using API.DTOs.Auth;
using API.Models.Identity;

namespace API.Interfaces.Service
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        int EmployeeId { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}
