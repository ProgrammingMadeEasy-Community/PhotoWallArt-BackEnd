using Domain.Interfaces;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users;

public class Role : IdentityRole<Guid>, IAuditable
{
    public Guid AccountId { get; set; }
    public string? Description { get; set; }
    public bool IsEditable { get; set; } = true;
    public bool IsActive { get; set; } = true;
    
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted => DeletedBy != null && DeletedOn != null;

    public List<Permission> Permissions { get; set; } = null!;
    public Role(string name) : base(name)
    {  
    }

    public Role(Guid accountId, string name, string? desctiption = null)
         : base(name)
    {
        Id = Guid.NewGuid();
        Description = desctiption;
        NormalizedName = name.ToUpperInvariant();
    }
}