using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Users
{
    [Index(nameof(RoleId), nameof(Module), nameof(Resource), nameof(Action), IsUnique = true)]
    public class Permission : Entity<Guid>
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public string Module { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string PermissionValue { get; set; } = string.Empty;

        public Permission()
        {
            Id = Guid.NewGuid();
        }

        public Permission(Guid accountId, Guid roleId, string module, string resource, string action, string value)
        {
            Id = Guid.NewGuid();
            RoleId = roleId;
            Module = module;
            Resource = resource;
            Action = action;
            PermissionValue = value;
        }
    }
}
