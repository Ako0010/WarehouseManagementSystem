using Microsoft.AspNetCore.Identity;

namespace WarehouseManagementSystem.Model;

public class User : IdentityUser
{
    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public string Role { get; set; } 

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
