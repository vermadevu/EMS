using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.DTOs.Auth;
using Xunit;

namespace API.Tests.Integration;

public class AuthenticationIntegrationTests(ApiWebApplicationFactory factory)
    : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ProtectedEndpoint_ShouldReturn401_WhenNotAuthenticated()
    {
        var response = await _client.GetAsync("/api/department");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldReturn401_WhenPasswordIsInvalid()
    {
        var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDto
        {
            Email = "admin@dems.com",
            Password = "WrongPassword@123"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldReturn200_WhenCredentialsAreValid()
    {
        var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDto
        {
            Email = "admin@dems.com",
            Password = "Admin@123"
        });

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        });

        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result!.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
    }

    [Fact]
    public async Task Me_ShouldReturn200_WhenAccessTokenIsValid()
    {
        var login = await LoginAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", login.AccessToken);

        var response = await _client.GetAsync("/api/account/me");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CurrentUserDto>(JsonOptions());
        Assert.Equal("admin@dems.com", result!.Email);
    }

    [Fact]
    public async Task Refresh_ShouldReturnNewAccessAndRefreshTokens()
    {
        var login = await LoginAsync();

        var response = await _client.PostAsJsonAsync("/api/account/refresh", new RefreshRequestDto
        {
            RefreshToken = login.RefreshToken
        });

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RefreshResponseDto>(JsonOptions());
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result!.AccessToken));
        Assert.NotEqual(login.RefreshToken, result.RefreshToken);
    }

    [Fact]
    public async Task Refresh_ShouldReturn401_WhenOldRefreshTokenIsReused()
    {
        var login = await LoginAsync();
        var rotationResponse = await _client.PostAsJsonAsync("/api/account/refresh", new RefreshRequestDto
        {
            RefreshToken = login.RefreshToken
        });
        rotationResponse.EnsureSuccessStatusCode();

        var response = await _client.PostAsJsonAsync("/api/account/refresh", new RefreshRequestDto
        {
            RefreshToken = login.RefreshToken
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private async Task<LoginResponseDto> LoginAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDto
        {
            Email = "admin@dems.com",
            Password = "Admin@123"
        });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<LoginResponseDto>(JsonOptions()))!;
    }

    private static JsonSerializerOptions JsonOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };
    }
}