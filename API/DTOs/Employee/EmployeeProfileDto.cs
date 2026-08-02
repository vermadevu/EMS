public class EmployeeProfileDto
{
    // Read Only
    public string EmployeeCode { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateOnly JoiningDate { get; set; }

    public string DepartmentName { get; set; } = "";
    public string DesignationName { get; set; } = "";
    public string? ManagerName { get; set; }
    public string? ProfileImage { get; set; }

    // Editable
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }

    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodGroup { get; set; }

    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactRelationship { get; set; }
    public string? EmergencyContactPhone { get; set; }
}