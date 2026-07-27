using API.Models.Enums;

namespace API.DTOs.Dashboard;
public class DashboardWidgetDto
{
    public DashboardWidgetType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public int Width { get; set; }
    public object? Data { get; set; }
}