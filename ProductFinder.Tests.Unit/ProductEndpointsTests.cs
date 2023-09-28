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
        var result = _sut.GetAll(_productService);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllProducts_ReturnsProduct_WhenProductExists()
    {
        //Arrange
        var products = new List<Product>
        {
            _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Green)
                .Create(),
            _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Blue)
                .Create()
        };
        _productService.GetAll().Returns(products);

        //Act
        var result = _sut.GetAll(_productService);

        //Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(2);
        result.Should().ContainSingle(x => x.ProductColor == ProductColor.Blue);
        result.Should().ContainSingle(x => x.ProductColor == ProductColor.Green);
    }

    [Fact]
    public void GetProductsByColor_ReturnsProduct_WhenProductWithColorExists()
    {
        //Arrange
        var products = new List<Product>
        {
            _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Blue)
                .Create()
        };
        _productService.GetByColor(Arg.Is(ProductColor.Blue)).Returns(products);

        //Act
        var result = _sut.GetProductsByColor(ProductColor.Blue.ToString(), _productService);

        //Assert
        result.Should().HaveCount(1);
        result.Should().ContainSingle(x => x.ProductColor == ProductColor.Blue);
    }

    [Fact]
    public void GetProductsByColor_ReturnsEmptyProducts_WhenProductColorNotExists()
    {
        //Arrange
        var products = new List<Product>
        {
            _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Blue)
                .Create()
        };
        _productService.GetByColor(Arg.Is(ProductColor.Blue)).Returns(products);

        //Act
        var result = _sut.GetProductsByColor("Yellow", _productService);

        //Assert
        result.Should().BeEmpty();
    }
}