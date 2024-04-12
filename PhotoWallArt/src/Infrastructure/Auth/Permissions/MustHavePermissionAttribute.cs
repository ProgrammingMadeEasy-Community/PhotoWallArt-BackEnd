using Microsoft.AspNetCore.Authorization;
using PhotoWallArt.Shared.Authorization;

namespace PhotoWallArt.Infrastructure.Auth.Permissions;
public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = FSHPermission.NameFor(action, resource);
}