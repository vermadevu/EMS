using API.Models.Identity;
using API.Services;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace API.Tests.Services;

public class TokenServiceTests
{
    private readonly TokenService _service = new(new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "ThisIsATestSigningKeyThatIsLongEnoughForHmacSha512SecurityAndHasMoreThanSixtyFourBytes123",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["Jwt:DurationInMinutes"] = "15"
        })
        .Build());

    [Fact]
    public async Task CreateTokenAsync_ShouldIncludeUserAndRoleClaims()
    {
        var token = await _service.CreateTokenAsync(new ApplicationUser
        {
            Id = "user-1",
            Email = "user@test.com",
            UserName = "user@test.com",
            EmployeeId = 7
        }, ["Admin", "HR"]);

        var parsed = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Equal("TestIssuer", parsed.Issuer);
        Assert.Equal("TestAudience", parsed.Audiences.Single());
        Assert.Equal("user-1", parsed.Claims.Single(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
        Assert.Contains(parsed.Claims, x => x.Type == ClaimTypes.Role && x.Value == "Admin");
        Assert.Contains(parsed.Claims, x => x.Type == ClaimTypes.Role && x.Value == "HR");
        Assert.Contains(parsed.Claims, x => x.Type == "EmployeeId" && x.Value == "7");
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnUniqueNonEmptyTokens()
    {
        var first = _service.GenerateRefreshToken();
        var second = _service.GenerateRefreshToken();

        Assert.False(string.IsNullOrWhiteSpace(first));
        Assert.NotEqual(first, second);
    }
}