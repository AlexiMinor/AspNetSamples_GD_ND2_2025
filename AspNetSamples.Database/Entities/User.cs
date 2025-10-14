namespace AspNetSamples.Database.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public DateTime? LastLogin { get; set; }
    public int FailedLoginAttempts { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; }
}