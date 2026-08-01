using API.Models.Enums;

namespace API.DTOs.Asset;

public class AssetDto
{
    public int Id { get; set; }
    public string AssetCode { get; set; } = "";
    public string AssetName { get; set; } = "";
    public AssetType AssetType { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public AssetStatus Status { get; set; }
    public string? EmployeeCode { get; set; }
    public int? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
}