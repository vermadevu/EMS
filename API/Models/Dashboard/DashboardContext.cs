using API.Models.Identity;

namespace API.Models.Dashboard;

public class DashboardContext
{
    public required ApplicationUser User { get; init; }
    public required HashSet<string> Permissions { get; init; }
}