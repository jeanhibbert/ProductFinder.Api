using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using ProductFinder.Api.EndpointDefinitions;
using ProductFinder.Api.Models;
using ProductFinder.Api.Services;

namespace ProductFinder.Tests.Unit;

public class ProductEndpointDefinitionTests
{
    private readonly IProductService _productService =
        Substitute.For<IProductService>();

    private readonly ProductEndpointDefinition _sut = new();

    [Fact]
    public void GetAllProducts_ReturnEmptyList_WhenNoProductsExist()
    {
        //Arrange
        _productService.GetAll().Returns(new List<Product>());

        //Act
        var result = _sut.GetFilteredProducts(_productService, Substitute.For<HttpContext>());

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllProducts_ReturnsProduct_WhenProductExists()
    {
        //Arrange
        Fixture fixture = new Fixture();
        var product = fixture.Create<Product>();
        _productService.GetAll().Returns(new List<Product> { product });

        //Act
        var result = _sut.GetFilteredProducts(_productService, Substitute.For<HttpContext>());

        //Assert
        result.Should().ContainSingle(x => x.Id == product.Id && x.ProductColor == product.ProductColor);
    }
}