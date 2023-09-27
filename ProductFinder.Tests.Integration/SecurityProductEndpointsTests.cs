using ProductFinder.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace ProductFinder.Tests.Integration;

public class SecurityProductEndpointsTests
{
    private readonly Fixture _fixture;

    public SecurityProductEndpointsTests()
    {
        _fixture = new Fixture();
    }

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

    [Fact]
    public async Task CreateProduct_WillReturnUnauthorized_WhenNotAuthenticated()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.PostAsJsonAsync("/api/products", _fixture.Create<Product>());

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetProductById_WillReturnUnauthorized_WhenNotAuthenticated()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"/api/products/{Guid.NewGuid}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
