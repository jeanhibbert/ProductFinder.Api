using ProductFinder.Persistence;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductFinder.Tests.Integration.Security;
internal static class SecurityUtil
{
    public static async Task<HttpClient> SetAuthorizationHeaderAsync(HttpClient httpClient)
    {
        var testUser = new MyTester($"test@email{Guid.NewGuid()}.com", "P@$$w0rd1");
        var registerResponse = await httpClient.PostAsJsonAsync("/register", testUser);
        registerResponse.EnsureSuccessStatusCode();
        var loginResponse = await httpClient.PostAsJsonAsync("/login", testUser);
        var loginResponseText = await loginResponse.Content.ReadAsStringAsync();
        var accessTokenResponse = JsonSerializer.Deserialize<AccessTokenResponse>(loginResponseText);
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessTokenResponse!.AccessToken}");
        return httpClient;
    }
}

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