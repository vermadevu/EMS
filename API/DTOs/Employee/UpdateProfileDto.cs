namespace API.DTOs.Employee
{
    public class UpdateProfileDto
    {
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
}
