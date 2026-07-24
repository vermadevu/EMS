using API.Models.Base;
using API.Models.Entities;
using API.Models.Enums;

namespace API.Models.Entities;

public class Asset : BaseEntity
{
    public string AssetCode { get; set; } = "";
    public string AssetName { get; set; } = "";
    public AssetType AssetType { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public AssetStatus Status { get; set; } = AssetStatus.Available;

    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}