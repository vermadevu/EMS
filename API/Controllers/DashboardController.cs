using API.Authorization;
using API.DTOs.Dashboard;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DashboardController(IDashboardService dashboardService)
    : BaseApiController
{
    private readonly IDashboardService _dashboardService = dashboardService;

    [HttpGet]
    [HasPermission(Permissions.Dashboard.View)]
    public async Task<ActionResult<DashboardDto>> GetDashboard()
    {
        return Ok(await _dashboardService.GetDashboardAsync());
    }
}