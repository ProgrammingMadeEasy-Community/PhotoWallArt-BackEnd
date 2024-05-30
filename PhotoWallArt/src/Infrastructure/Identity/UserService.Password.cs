using Mapster;
using Microsoft.AspNetCore.WebUtilities;
using PhotoWallArt.Application.Common.Exceptions;
using PhotoWallArt.Application.Common.Mailing;
using PhotoWallArt.Application.Common.ResponseObject;
using PhotoWallArt.Application.Identity.ResponseFactory;
using PhotoWallArt.Application.Identity.Users;
using PhotoWallArt.Application.Identity.Users.Password;

namespace PhotoWallArt.Infrastructure.Identity;
internal partial class UserService
{
    public async Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        EnsureValidTenant();

        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
        //if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        //{
        //    // Don't reveal that the user does not exist or is not confirmed
        //    return UserMgtResponse.UserNotFoundResponse();
        //    throw new InternalServerException(_t["An Error has occurred!"]);
        //}

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        string code = await _userManager.GeneratePasswordResetTokenAsync(user);
        const string route = "account/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
        var mailRequest = new MailRequest(
            new List<string> { request.Email },
            _t["Reset Password"],
            _t[$"Your Password Reset Token is '{code}'. You can reset your password using the {endpointUri} Endpoint."]);
        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));

        return UserMgtResponse.ForgotPassWordSuccessResponse();
    }

    public async Task<ApiResponse<UserDetailsDto>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email?.Normalize()!);

        // Don't reveal that the user does not exist
        if (user == null)
        {
            return UserMgtResponse.UserNotFoundResponse();
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token!, request.Password!);

        var data = user.Adapt<UserDetailsDto>();

        if (!result.Succeeded)
        {
            return UserMgtResponse.UpdateValidationError(result.GetErrors(_t).ToArray());
        }

        return UserMgtResponse.PassWordUpdateSuccessResponse(data);
    }

    public async Task<ApiResponse<UserDetailsDto>> ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if(user == null)
        {
            UserMgtResponse.UserNotFoundResponse();
        }

        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

        var data = user.Adapt<UserDetailsDto>();

        if (!result.Succeeded)
        {
            return UserMgtResponse.UpdateValidationError(result.GetErrors(_t).ToArray());
        }

        return UserMgtResponse.PassWordUpdateSuccessResponse(data);
    }
}