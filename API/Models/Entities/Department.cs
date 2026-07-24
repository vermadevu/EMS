using API.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Entities;

public class Department : BaseEntity
{
    [Required]
    public string Name { get; set; } = "";
    public string? Description { get; set; }

    //Navigation Property

    // 1 Department ------------ * Employees
    public ICollection<Employee> Employees { get; set; } =  [];
}
