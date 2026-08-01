using API.Models.Enums;

namespace API.DTOs.Asset
{
    public class AssetListItemDto
    {
        public int Id { get; set; }
        public string AssetCode { get; set; } = "";
        public string AssetName { get; set; } = "";
        public AssetType AssetType { get; set; }
        public AssetStatus Status { get; set; }
        public string? EmployeeName { get; set; }
    }
}
