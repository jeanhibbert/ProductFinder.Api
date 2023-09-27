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

    private readonly Fixture _fixture;

    private readonly ProductEndpointDefinition _sut = new();

    public ProductEndpointDefinitionTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void GetAllProducts_ReturnEmptyList_WhenNoProductsExist()
    {
        //Arrange
        _productService.GetAll().Returns(new List<Product>());

        //Act
        var result = _sut.GetFilteredProducts(_productService, Substitute.For<IHttpContextAccessor>());

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllProducts_ReturnsProduct_WhenProductExists()
    {
        //Arrange
        var product = _fixture.Create<Product>();
        _productService.GetAll().Returns(new List<Product> { product });

        //Act
        var result = _sut.GetFilteredProducts(_productService, Substitute.For<IHttpContextAccessor>());

        //Assert
        result.Should().ContainSingle(x => x.Id == product.Id && x.ProductColor == product.ProductColor);
    }
}