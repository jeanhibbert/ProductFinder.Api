using ProductFinder.Api.Models;
using System.Net;

namespace ProductFinder.Tests.Integration;

public class SecurityProductEndpointsTests
{

    [Fact]
    public async Task GetAllProducts_WillReturnUnauthorized_WhenNotAuthenticated()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"/api/products");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetProductsByColor_WillReturnUnauthorized_WhenNotAuthenticated()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"/api/products?color={ProductColor.Blue}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
