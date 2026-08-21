using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using API.DTOs.Auth;
using Xunit;

namespace API.Tests.Integration;

public class CoreApiIntegrationTests(ApiWebApplicationFactory factory)
    : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory = factory;

    [Fact]
    public async Task GetDepartments_ShouldReturn200ForAdmin()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/department/all");

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetDepartment_ShouldReturn404_WhenDepartmentDoesNotExist()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/department/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeStatuses_ShouldReturnAllDefinedStatuses()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/employee/statuses");

        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(5, document.RootElement.GetArrayLength());
    }

    [Fact]
    public async Task GetEmployees_ShouldReturnPagedResponseForAdmin()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/employee?pageNumber=1&pageSize=10");

        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(document.RootElement.TryGetProperty("items", out _));
        Assert.True(document.RootElement.TryGetProperty("totalCount", out _));
    }

    [Fact]
    public async Task GetAssets_ShouldReturnPagedResponseForAdmin()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/asset?pageNumber=1&pageSize=10");

        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(document.RootElement.TryGetProperty("items", out _));
        Assert.True(document.RootElement.TryGetProperty("totalCount", out _));
    }

    private async Task<HttpClient> CreateAdminClientAsync()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/account/login", new LoginDto
        {
            Email = "admin@dems.com",
            Password = "Admin@123"
        });
        response.EnsureSuccessStatusCode();

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var token = document.RootElement.GetProperty("accessToken").GetString();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}