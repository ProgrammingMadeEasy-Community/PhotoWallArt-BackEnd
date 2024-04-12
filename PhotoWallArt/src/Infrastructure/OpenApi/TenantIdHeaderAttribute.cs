using PhotoWallArt.Shared.Multitenancy;

namespace PhotoWallArt.Infrastructure.OpenApi;
public class TenantIdHeaderAttribute : SwaggerHeaderAttribute
{
    public TenantIdHeaderAttribute()
        : base(
            MultitenancyConstants.TenantIdName,
            "Input your tenant Id to access this API",
            "Root",
            true)
    {
    }
}
