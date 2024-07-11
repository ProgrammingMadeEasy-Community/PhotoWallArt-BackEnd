namespace PhotoWallArt.Application.Identity.Tokens;

public class TokenResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public string Id { get; set; }
    public string FirstName { get; set; }
    public string Lastname { get; set; }
    public string ImageUrl { get; set; }
    public string Email { get; set; }
    public string[] Roles { get; set; }
    public string PhoneNumber { get; set; }

}