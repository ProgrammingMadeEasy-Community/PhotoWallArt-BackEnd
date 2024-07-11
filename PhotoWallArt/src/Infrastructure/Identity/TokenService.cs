using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhotoWallArt.Application.Common.Exceptions;
using PhotoWallArt.Application.Common.ResponseObject;
using PhotoWallArt.Application.Identity.ResponseFactory;
using PhotoWallArt.Application.Identity.Tokens;
using PhotoWallArt.Infrastructure.Auth;
using PhotoWallArt.Infrastructure.Auth.Jwt;
using PhotoWallArt.Infrastructure.Multitenancy;
using PhotoWallArt.Shared.Authorization;
using PhotoWallArt.Shared.Multitenancy;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PhotoWallArt.Infrastructure.Identity;
internal class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer _t;
    private readonly SecuritySettings _securitySettings;
    private readonly JwtSettings _jwtSettings;
    private readonly FSHTenantInfo? _currentTenant;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IStringLocalizer<TokenService> localizer,
        FSHTenantInfo? currentTenant,
        IOptions<SecuritySettings> securitySettings)
    {
        _userManager = userManager;
        _t = localizer;
        _jwtSettings = jwtSettings.Value;
        _currentTenant = currentTenant;
        _securitySettings = securitySettings.Value;
    }


    public async Task<ApiResponse<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_currentTenant?.Id)
            || await _userManager.FindByEmailAsync(request.Email.Trim().Normalize()) is not { } user
            || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return UserMgtResponse.UnauthorizedResponse(_t["Authentication Failed. Please confirm details"]);
        }

        if (!user.IsActive)
        {
            return UserMgtResponse.UnauthorizedResponse(_t["Authentication Failed. User not active, please contact the Amin"]);
        }

        if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        {
            return UserMgtResponse.UnauthorizedResponse(_t["E-Mail not confirmed."]);
        }
        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    public async Task<ApiResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        string? userEmail = userPrincipal.GetEmail();
        var user = await _userManager.FindByEmailAsync(userEmail!);
        if (user is null)
        {
            return UserMgtResponse.UnauthorizedResponse(_t["Authentication Failed. Please confirm details"]);
        }

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return UserMgtResponse.UnauthorizedResponse(_t["Invalid Refresh Token."]);
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    private async Task<ApiResponse<TokenResponse>> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
    {
        string token = GenerateJwt(user, ipAddress);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

        var userRoles = await _userManager.GetRolesAsync(user);

        string[] roles = new string[userRoles.Count];
        for (int i = 0; i < roles.Length; i++)
        {
            roles[i] = userRoles[i];
        }

        var tokenResponse = new TokenResponse
        {
            Token = token,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,

            Id = user.Id,
            ImageUrl = user.ImageUrl,
            FirstName = user.FirstName,
            Lastname = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Roles = roles
        };

        return UserMgtResponse.SuccessfulLogin(tokenResponse);
    }

    private string GenerateJwt(ApplicationUser user, string ipAddress) =>
        GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

    private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress) =>
        new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(FSHClaims.Fullname, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Name, user.FirstName ?? string.Empty),
            new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(FSHClaims.IpAddress, ipAddress),
            new(FSHClaims.Tenant, _currentTenant!.Id),
            new(FSHClaims.ImageUrl, user.ImageUrl ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        };

    private static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new UnauthorizedException(_t["Invalid Token."]);
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}