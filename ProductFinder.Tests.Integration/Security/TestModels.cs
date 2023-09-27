using System.Text.Json.Serialization;

namespace ProductFinder.Tests.Integration.Security;

public record MyTester(string email, string password);

public class AccessTokenResponse
{
    public string TokenType { get; } = "Bearer";

    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expiresIn")]
    public long ExpiresIn { get; set; }

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}