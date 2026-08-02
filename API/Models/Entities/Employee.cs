using API.Models.Base;
using API.Models.Enums;
using API.Models.Identity;

namespace API.Models.Entities;

public class Employee : BaseEntity
{
    public string EmployeeCode { get; set; } = "";
    public string FullName => $"{FirstName} {LastName}";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateOnly JoiningDate { get; set; }
    public EmployeeStatus Status { get; set; } =  EmployeeStatus.Pending;
    public string? ProfileImage { get; set; }


    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelationship { get; set; }



    // Foreign Keys
    public int DepartmentId { get; set; }

    public int DesignationId { get; set; }

    public int? ManagerId { get; set; }


    // Navigation Property

    public Department Department { get; set; } = null!;
    public Designation Designation { get; set; } = null!;
    public Employee? Manager { get; set; }
    public ICollection<Employee> TeamMembers { get; set; } = [];
    public ICollection<Document> Documents { get; set; } = [];
    public ICollection<Asset> Assets { get; set; } = [];
    public ApplicationUser? User { get; set; }
}
