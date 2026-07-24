using API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Asset;

public class CreateAssetDto
{
    [Required]
    public string AssetName { get; set; } = "";

    [Required]
    public AssetType AssetType { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public DateOnly PurchaseDate { get; set; }
}