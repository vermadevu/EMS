namespace API.Models.Base;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
