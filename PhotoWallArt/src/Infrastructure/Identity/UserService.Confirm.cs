using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using PhotoWallArt.Application.Common.Exceptions;
using PhotoWallArt.Application.Common.ResponseObject;
using PhotoWallArt.Application.Identity.ResponseFactory;
using PhotoWallArt.Infrastructure.Common;
using PhotoWallArt.Shared.Multitenancy;
using System.Text;

namespace PhotoWallArt.Infrastructure.Identity;
internal partial class UserService
{
    private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
    {
        EnsureValidTenant();

        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/users/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, MultitenancyConstants.TenantIdName, _currentTenant.Id!);
        return verificationUri;
    }

    public async Task<ApiResponse> ConfirmEmailAsync(string userId, string code, string tenant, CancellationToken cancellationToken)
    {
        EnsureValidTenant();

        var user = await _userManager.Users
            .Where(u => u.Id == userId && !u.EmailConfirmed)
            .FirstOrDefaultAsync(cancellationToken);
        if(user == null)
        {
            return UserMgtResponse.EmailConfirmationFailed();
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if(!result.Succeeded)
        {
            return UserMgtResponse.NonGenericInternalServerError(result.GetErrors(_t).ToArray());
        }

        return UserMgtResponse.EmailConfirmationSuccessfull();
    }

    //public async Task<string> ConfirmPhoneNumberAsync(string userId, string code)
    //{
    //    EnsureValidTenant();

    //    var user = await _userManager.FindByIdAsync(userId);

    //    _ = user ?? throw new InternalServerException(_t["An error occurred while confirming Mobile Phone."]);
    //    if (string.IsNullOrEmpty(user.PhoneNumber)) throw new InternalServerException(_t["An error occurred while confirming Mobile Phone."]);

    //    var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);

    //    return result.Succeeded
    //        ? user.PhoneNumberConfirmed
    //            ? string.Format(_t["Account Confirmed for Phone Number {0}. You can now use the /api/tokens endpoint to generate JWT."], user.PhoneNumber)
    //            : string.Format(_t["Account Confirmed for Phone Number {0}. You should confirm your E-mail before using the /api/tokens endpoint to generate JWT."], user.PhoneNumber)
    //        : throw new InternalServerException(string.Format(_t["An error occurred while confirming {0}"], user.PhoneNumber));
    //}
}
