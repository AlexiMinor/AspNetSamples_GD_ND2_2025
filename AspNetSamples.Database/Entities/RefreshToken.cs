namespace AspNetSamples.Database.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    
    public DateTime? CreationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }

    //public string Token { get; set; } // GUID in string format can be used ID not this field
    public string? Device { get; set; }

    public bool IsRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

    public Guid UserId { get; set; }
    public User User { get; set; }

}