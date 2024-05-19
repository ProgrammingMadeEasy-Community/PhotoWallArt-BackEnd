using PhotoWallArt.Application.Common.ResponseObject;
using PhotoWallArt.Application.Identity.Tokens;
using PhotoWallArt.Application.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static ApiResponse<TokenResponse> RedirectResponse(TokenResponse Data)
    {
        return new ApiResponse<TokenResponse>
        {
            Message = "The request resulted in a redirection.",
            StatusCode = 302,
            Status = true,
            Data = Data
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

    public static ApiResponse<string> InternalServerErrorResponse()
    {
        return new ApiResponse<string>
        {
            Message = "An internal server error occurred while processing the request.",
            StatusCode = 500,
            Status = false,
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


