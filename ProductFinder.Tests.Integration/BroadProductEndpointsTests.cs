using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using ProductFinder.Api.Models;
using System.Text.Json;
using AutoFixture;

namespace ProductFinder.Tests.Integration;

public class BroadProductEndpointsTests
{
    private readonly Fixture _fixture;

    public BroadProductEndpointsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllProducts_WillReturnProducts_WhenExists()
    {
        //Arrange
        var product = _fixture.Create<Product>();

        using var app = new TestApplicationFactory();

        var httpClient = app.CreateClient();
        await httpClient.PostAsJsonAsync("/api/products", product);

        //Act
        var response = await httpClient.GetAsync($"/api/products");
        var responseText = await response.Content.ReadAsStringAsync();
        var productsResult = JsonSerializer.Deserialize<List<Product>>(responseText);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productsResult.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetProductsByColor_WillReturnProducts_WhenExists()
    {
        //Arrange
        var product = _fixture.Create<Product>();

        using var app = new TestApplicationFactory();

        var httpClient = app.CreateClient();
        await httpClient.PostAsJsonAsync("/api/products", product);

        //Act
        var response = await httpClient.GetAsync($"/api/products?color={product.ProductColor}");
        var responseText = await response.Content.ReadAsStringAsync();
        var productsResult = JsonSerializer.Deserialize<List<Product>>(responseText);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productsResult.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetProductById_ReturnProduct_WhenProductExists()
    {
        //Arrange
        var product = _fixture.Create<Product>();

        using var app = new TestApplicationFactory();

        var httpClient = app.CreateClient();
        await httpClient.PostAsJsonAsync("/api/products", product);

        //Act
        var response = await httpClient.GetAsync($"/api/products/{product.Id}");
        var responseText = await response.Content.ReadAsStringAsync();
        var productResult = JsonSerializer.Deserialize<Product>(responseText);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productResult.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetProductById_ReturnNotFound_WhenProductDoesNotExists()
    {
        //Arrange
        using var app = new TestApplicationFactory();

        var guid = Guid.NewGuid();
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"/api/products/{guid}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}