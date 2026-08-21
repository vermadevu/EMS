using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace API.Tests.Integration;

public class AuthorizationIntegrationTests(ApiWebApplicationFactory factory)
    : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Admin_ShouldAccessEmployeeManagement()
    {
        using var client = _factory.CreateClient();
        var token = await GetAccessTokenAsync(client, "admin@dems.com", "Admin@123");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/employee/all");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Employee_ShouldNotAccessAllAssetsWithoutAssetReadPermission()
    {
        using var client = _factory.CreateClient();
        var token = await _factory.CreateAccessTokenForRoleAsync("Employee");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/asset");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Employee_ShouldAccessOwnAssetsWithAssetReadOwnPermission()
    {
        using var client = _factory.CreateClient();
        var token = await _factory.CreateAccessTokenForRoleAsync("Employee");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/asset/me");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static async Task<string> GetAccessTokenAsync(
        HttpClient client,
        string email,
        string password)
    {
        var response = await client.PostAsJsonAsync("/api/account/login", new
        {
            Email = email,
            Password = password
        });
        response.EnsureSuccessStatusCode();

        using var document = System.Text.Json.JsonDocument.Parse(
            await response.Content.ReadAsStringAsync());
        return document.RootElement.GetProperty("accessToken").GetString()!;
    }
}