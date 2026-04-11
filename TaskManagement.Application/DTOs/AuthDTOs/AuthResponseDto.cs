namespace TaskManagement.Application.DTOs.AuthDTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }
    public string Email { get; set; } = string.Empty;
}
