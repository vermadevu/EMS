using API.Models.Base;

namespace API.Models.Entities;

public class Designation : BaseEntity
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }

    
    //Navigation Property

    // 1 Department ------------ * Employees
    public ICollection<Employee> Employees { get; set; } = [];
}
