using Domain.Entities.Users;
namespace Domain.Shared;

public class BaseUser : Entity<Guid>
{
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive => ApplicationUser.IsActive;
    public string Email => ApplicationUser.Email;
    public string PhoneNumber => ApplicationUser.PhoneNumber!;

    public BaseUser()
    {
        Id = Guid.NewGuid();
    }
}