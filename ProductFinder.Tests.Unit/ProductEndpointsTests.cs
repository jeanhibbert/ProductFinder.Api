using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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
    public void GetFilteredProducts_ReturnEmptyList_WhenNoProductsExist()
    {
        //Arrange
        _productService.GetAll().Returns(new List<Product>());

        //Act
        var result = _sut.GetFilteredProducts(_productService, Substitute.For<IHttpContextAccessor>());

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetFilteredProducts_ReturnsProduct_WhenProductExists()
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
        var result = _sut.GetFilteredProducts(_productService, Substitute.For<IHttpContextAccessor>());

        //Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(2);
        result.Should().ContainSingle(x => x.ProductColor == ProductColor.Blue);
        result.Should().ContainSingle(x => x.ProductColor == ProductColor.Green);
    }

    [Fact]
    public void GetFilteredProducts_ReturnsProduct_WhenProductWithColorExists()
    {
        //Arrange
        var products = new List<Product>
        {
            _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Blue)
                .Create()
        };
        _productService.GetByColor(Arg.Is(ProductColor.Blue)).Returns(products);

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        var queryParameters = new QueryCollection(new Dictionary<string, StringValues>
        {
            { "color", ProductColor.Blue.ToString() }
        });
        var httpContext = new DefaultHttpContext
        {
            Request = { Query = queryParameters }
        };
        httpContextAccessor.HttpContext.Returns(httpContext);

        //Act
        var result = _sut.GetFilteredProducts(_productService, httpContextAccessor);

        //Assert
        result.Should().HaveCount(1);
        result.Should().ContainSingle(x => x.ProductColor == ProductColor.Blue);
    }
}