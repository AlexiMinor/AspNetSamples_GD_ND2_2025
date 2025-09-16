namespace AspNetSamples.Core.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string PasswordHash { get; set; }
    public DateTime? LastLogin { get; set; }
    public int FailedLoginAttempts { get; set; }

    public Guid RoleId { get; set; }
    public RoleDto? Role { get; set; }
}