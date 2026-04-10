using Microsoft.AspNetCore.Identity;

namespace WarehouseManagementSystem.Domain.Model;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public bool IsOnline { get; set; }
    public DateTime? LastSeen { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
