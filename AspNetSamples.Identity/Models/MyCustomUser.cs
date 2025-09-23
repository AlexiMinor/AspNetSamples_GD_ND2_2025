using Microsoft.AspNetCore.Identity;

namespace AspNetSamples.Identity.Models;

public class MyCustomUser : IdentityUser<Guid>
{
    public string CustomTag { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }

}