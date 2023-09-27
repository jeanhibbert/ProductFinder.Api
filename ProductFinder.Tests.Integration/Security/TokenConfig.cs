using System.Net.Http.Json;
using System.Text.Json;

namespace ProductFinder.Tests.Integration.Security;

public class TokenConfig : IAsyncLifetime
{
    public string TestUserToken { get; private set; }
    private HttpClient _httpClient;

    public async Task InitializeAsync()
    {
        using var app = new TestApplicationFactory();
        _httpClient = app.CreateClient();
        TestUserToken = await RegisterTestUser(_httpClient);
    }

    public Task DisposeAsync()
    {
        _httpClient.Dispose();
        return Task.CompletedTask;
    }

    private async Task<string> RegisterTestUser(HttpClient httpClient)
    {
        var testUser = new MyTester($"test@email.com", "P@$$w0rd1");
        var registerResponse = await httpClient.PostAsJsonAsync("/register", testUser);
        registerResponse.EnsureSuccessStatusCode();
        var loginResponse = await httpClient.PostAsJsonAsync("/login", testUser);
        var loginResponseText = await loginResponse.Content.ReadAsStringAsync();
        var accessTokenResponse = JsonSerializer.Deserialize<AccessTokenResponse>(loginResponseText);
        return accessTokenResponse.AccessToken;
    }
}