
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users;

public class ApplicationUser : IdentityUser<Guid>
{
    public new string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public Guid AccountId { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public ApplicationUser()
    {
    }

    public ApplicationUser(Guid accountId, string email, string phone)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Email = email;
        NormalizedEmail = Email.ToUpper();
        UserName = email;
        NormalizedUserName = UserName.ToUpper();
        PhoneNumber = phone;
        IsActive = true;
        EmailConfirmed = false;
        PhoneNumberConfirmed = false;
        TwoFactorEnabled = false;
        LockoutEnabled = false;
        AccessFailedCount = 0;
    }
}