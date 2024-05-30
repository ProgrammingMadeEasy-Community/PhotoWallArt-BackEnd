using PhotoWallArt.Application.Common.ResponseObject;
using PhotoWallArt.Application.Identity.Tokens;
using PhotoWallArt.Application.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhotoWallArt.Application.Identity.ResponseFactory;
public class UserMgtResponse
{
    public static ApiResponse<TokenResponse> SuccessfulLogin(TokenResponse data)
    {
        return new ApiResponse<TokenResponse>
        {
            Message = "User logged in successfully.",
            StatusCode = 200,
            Status = true,
            Data = data
        };
    }

    public static ApiResponse<TokenResponse> UnauthorizedResponse(string message)
    {
        return new ApiResponse<TokenResponse>
        {
            Message = message,
            StatusCode = 401,
            Status = false
        };
    }

    public static ApiResponse<List<UserDetailsDto>> FetchAllUsersSuccessResponse(List<UserDetailsDto> data)
    {
        return new ApiResponse<List<UserDetailsDto>>
        {
            Message = "Request was successful.",
            StatusCode = 200,
            Status = true,
            Data = data
        };
    }

    public static ApiResponse<UserDetailsDto> FetchUserSuccessResponse(UserDetailsDto data)
    {
        return new ApiResponse<UserDetailsDto>
        {
            Message = "User fetch Request was successful.",
            StatusCode = 200,
            Status = true,
            Data = data
        };
    }

    public static ApiResponse<UserDetailsDto> UserNotFoundResponse()
    {
        return new ApiResponse<UserDetailsDto>
        {
            Message = "The requested User not found.",
            StatusCode = 404,
            Status = false
        };
    }

    public static ApiResponse UserRegSuccessResponse()
    {
        return new ApiResponse
        {
            Message = "Your registration was Successful, a link for email verification has been sent your mail.",
            StatusCode = 201,
            Status = true
        };
    }

    public static ApiResponse UserCreateSuccessResponse()
    {
        return new ApiResponse
        {
            Message = "User Created Successfully.",
            StatusCode = 201,
            Status = true
        };
    }

    public static ApiResponse<UserDetailsDto> UserUpdateSuccessResponse(UserDetailsDto data)
    {
        return new ApiResponse<UserDetailsDto>
        {
            Message = "User fetch Request was successful.",
            StatusCode = 200,
            Status = true,
            Data = data
        };
    }

    public static ApiResponse<UserDetailsDto> PassWordUpdateSuccessResponse(UserDetailsDto data)
    {
        return new ApiResponse<UserDetailsDto>
        {
            Message = "User fetch Request was successful.",
            StatusCode = 200,
            Status = true,
            Data = data
        };
    }

    public static ApiResponse ForgotPassWordSuccessResponse()
    {
        return new ApiResponse
        {
            Message = "An email has been sent to your mail, if not seen, please return and verify the email you supplied",
            StatusCode = 200,
            Status = true,
        };
    }



    public static ApiResponse CreateValidationError(string[]? errors)
    {
        return new ApiResponse
        {
            Message = "Validation Error(s) Occured",
            StatusCode = 400,
            Status = false,
            Errors = errors
        };
    }

    public static ApiResponse<UserDetailsDto> UpdateValidationError(string[]? errors)
    {
        return new ApiResponse<UserDetailsDto>
        {
            Message = "Update Failed Validation Error(s) Occured",
            StatusCode = 400,
            Status = false,
            Errors = errors
        };
    }

    public static ApiResponse<UserDetailsDto> UpdateFailed(string[]? errors)
    {
        return new ApiResponse<UserDetailsDto>
        {
            Message = "Update failed! Validation Error(s) Occured",
            StatusCode = 304,
            Status = true,
            Errors = errors
        };
    }

    public static ApiResponse EmailConfirmationSuccessfull()
    {
        return new ApiResponse
        {
            Message = "Email confirmation succeeded, you can go to the login page now",
            StatusCode = 200,
            Status = true,
        };
    }

    public static ApiResponse EmailConfirmationFailed()
    {
        return new ApiResponse
        {
            Message = "Email confirmation fialed, an error occured, please check the email and try again",
            StatusCode = 404,
            Status = false,
        };
    }

    public static ApiResponse NonGenericInternalServerError(string[] errors)
    {
        return new ApiResponse
        {
            Message = "An Error Occured",
            StatusCode = 500,
            Status = false,
            Errors = errors
        };
    }

    public static ApiResponse<UserDetailsDto> GenericInternalServerError(string[] errors)
    {
        return new ApiResponse<UserDetailsDto>
        {
            Message = "An Error Occured",
            StatusCode = 500,
            Status = false,
            Errors = errors
        };
    }


    public static ApiResponse RedirectResponse()
    {
        return new ApiResponse
        {
            Message = "The request resulted in a redirection.",
            StatusCode = 302,
            Status = true,
        };
    }

    public static ApiResponse<TokenResponse> ResourceFoundResponse(TokenResponse data)
    {
        return new ApiResponse<TokenResponse>
        {
            Message = "The requested resource was found.",
            StatusCode = 200,
            Status = true,
            Data = data
        };
    }

    public static ApiResponse<TokenResponse> SuccessResponse(TokenResponse data)
    {
        return new ApiResponse<TokenResponse>
        {
            Message = "Request was successful.",
            StatusCode = 200,
            Status = true,
            Data = data
        };
    }
}


