using PhotoWallArt.Application.Common.ResponseObject;
using PhotoWallArt.Application.Identity.Users.Password;
using System.Security.Claims;

namespace PhotoWallArt.Application.Identity.Users;
public interface IUserService : ITransientService
{
    Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);

    Task<bool> ExistsWithNameAsync(string name);
    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);

    Task<ApiResponse<List<UserDetailsDto>>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<ApiResponse<UserDetailsDto>> GetAsync(string userId, CancellationToken cancellationToken);

    Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken);
    Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken);

    Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken);
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);
    Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken);

    Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

    Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    Task<ApiResponse> CreateAsync(CreateUserRequest request, string origin);
    Task<ApiResponse<UserDetailsDto>> UpdateAsync(UpdateUserRequest request, string userId);

    Task<ApiResponse> ConfirmEmailAsync(string userId, string code, string tenant, CancellationToken cancellationToken);
   // Task<string> ConfirmPhoneNumberAsync(string userId, string code);

    Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
    Task<ApiResponse<UserDetailsDto>> ResetPasswordAsync(ResetPasswordRequest request);
    Task<ApiResponse<UserDetailsDto>> ChangePasswordAsync(ChangePasswordRequest request, string userId);
}