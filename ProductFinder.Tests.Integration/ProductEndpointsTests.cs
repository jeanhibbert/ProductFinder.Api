using ProductFinder.Api.Models;
using ProductFinder.Tests.Integration.Security;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductFinder.Tests.Integration;

public class ProductEndpointsTests : IClassFixture<TokenConfig>
{
    private readonly Fixture _fixture;
    private readonly string _testUserAccessToken;

    public ProductEndpointsTests(TokenConfig fixture)
    {
        _fixture = new Fixture();
        _testUserAccessToken = fixture.TestUserToken;
    }

    [Fact]
    public async Task GetAllProducts_WillReturnProducts_WhenExists()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var httpClient = app.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_testUserAccessToken}");

        await httpClient.PostAsJsonAsync("/api/products", _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Blue)
                .Create());
        await httpClient.PostAsJsonAsync("/api/products", _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Green)
                .Create());

        //Act
        var response = await httpClient.GetAsync($"/api/products");
        var responseText = await response.Content.ReadAsStringAsync();
        var productsResult = JsonSerializer.Deserialize<List<Product>>(responseText);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productsResult.Should().NotBeNullOrEmpty();
        productsResult.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllProducts_WillReturnEmptyProducts_WhenNonExists()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var httpClient = app.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_testUserAccessToken}");

        //Act
        var response = await httpClient.GetAsync($"/api/products");
        var responseText = await response.Content.ReadAsStringAsync();
        var productsResult = JsonSerializer.Deserialize<List<Product>>(responseText);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productsResult.Should().BeEmpty();
    }

    [Fact]
    public async Task GetProductsByColor_WillReturnProducts_WhenExists()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var httpClient = app.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_testUserAccessToken}");

        await httpClient.PostAsJsonAsync("/api/products", _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Blue)
                .Create());
        await httpClient.PostAsJsonAsync("/api/products", _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Blue)
                .Create());
        await httpClient.PostAsJsonAsync("/api/products", _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Green)
                .Create());
        await httpClient.PostAsJsonAsync("/api/products", _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.White)
                .Create());

        //Act
        var response = await httpClient.GetAsync($"/api/products/{ProductColor.Blue}");
        var responseText = await response.Content.ReadAsStringAsync();
        var productsResult = JsonSerializer.Deserialize<List<Product>>(responseText);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productsResult.Should().NotBeNullOrEmpty();
        productsResult.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetProductsByColor_WillReturnNoProducts_WhenColorNotExists()
    {
        //Arrange
        var product = _fixture.Create<Product>();
        using var app = new TestApplicationFactory();
        var httpClient = app.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_testUserAccessToken}");

        await httpClient.PostAsJsonAsync("/api/products", product);

        //Act
        var response = await httpClient.GetAsync($"/api/products/pink");
        var responseText = await response.Content.ReadAsStringAsync();
        var productsResult = JsonSerializer.Deserialize<List<Product>>(responseText);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productsResult.Should().BeEmpty();
    }
}