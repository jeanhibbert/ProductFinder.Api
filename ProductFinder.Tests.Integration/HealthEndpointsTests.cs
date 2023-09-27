using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace ProductFinder.Tests.Integration;
public class HealthEndpointsTests
{
    [Fact]
    public async Task CanPerformHealthCheck_WillReturnHealthy_WhenReady()
    {
        //Arrange
        using var app = new TestApplicationFactory();

        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"/health/ready");
        var responseText = await response.Content.ReadAsStringAsync();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseText.Should().Be(HealthStatus.Healthy.ToString());
    }

    [Fact]
    public async Task CanPerformHealthCheck_WillReturnHealthy_WhenLive()
    {
        //Arrange
        using var app = new TestApplicationFactory();

        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"/health/live");
        var responseText = await response.Content.ReadAsStringAsync();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseText.Should().Be(HealthStatus.Healthy.ToString());
    }
}
