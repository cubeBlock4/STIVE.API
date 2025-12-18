namespace Infrastructure.Entities;

public class CustomerEntity : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Customer";
}