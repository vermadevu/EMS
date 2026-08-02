using System.Security.Claims;
namespace API.Helpers;
public static class ClaimsPrincipalExtensions
{
    public static int GetEmployeeId(this ClaimsPrincipal user)
    {
        return int.Parse(
            user.FindFirstValue("EmployeeId")!
        );
    }
}
