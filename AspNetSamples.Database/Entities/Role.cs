namespace AspNetSamples.Database.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    //public string EngName { get; set; }

    public ICollection<User> Users { get; set; }
}