namespace API.DTOs.User;

public class CreateUserResponseDto
{
    public string Username { get; set; } = string.Empty;
    public string TemporaryPassword { get; set; } = string.Empty;
}