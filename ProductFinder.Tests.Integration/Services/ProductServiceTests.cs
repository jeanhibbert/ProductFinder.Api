using ProductFinder.Api.Models;
using ProductFinder.Api.Services;

namespace ProductFinder.Tests.Integration.Services;
public class ProductServiceTests
{
    private readonly Fixture _fixture;

    public ProductServiceTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void GetAllProducts_WillReturnProducts_WhenProductsExist()
    {
        //Arrange
        var productData = _fixture.Create<List<Product>>().ToDictionary(product => product.Id);
        var productService = new ProductService(productData);

        //Act
        var products = productService.GetAll();

        //Assert
        products.Count.Should().Be(productData.Count);
        foreach (ProductColor productColor in Enum.GetValues(typeof(ProductColor)))
        {
            products.Count(x => x.ProductColor == productColor)
                .Should().Be(productData.Values.Count(x => x.ProductColor == productColor));            
        }
    }

    [Fact]
    public void GetAllProducts_WillReturnEmptyList_WhenNoProductsExist()
    {
        //Arrange
        var productData = new List<Product>().ToDictionary(product => product.Id);
        var productService = new ProductService(productData);

        //Act
        var products = productService.GetAll();

        //Assert
        products.Should().BeEmpty();
    }

    [Fact]
    public void GetProductByColor_WillReturnProducts_WhenProductsWithColorExist()
    {
        //Arrange
        var productData = new List<Product> 
        {
             _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Green)
                .Create(),
              _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.Green)
                .Create(),
               _fixture.Build<Product>()
                .With(p => p.ProductColor, ProductColor.White)
                .Create(),
        }.ToDictionary(product => product.Id);
        var productService = new ProductService(productData);

        //Act
        var products = productService.GetByColor(ProductColor.Green);

        //Assert
        products.Count.Should().Be(2);
        products.Any(x => x.ProductColor != ProductColor.Green).Should().BeFalse();
    }

    [Fact]
    public void GetProductByColor_WillReturnNoProducts_WhenNoProductsWithColorExist()
    {
        //Arrange
        var productData = new List<Product>().ToDictionary(product => product.Id);
        var productService = new ProductService(productData);

        //Act
        var products = productService.GetByColor(ProductColor.Green);

        //Assert
        products.Should().BeEmpty();
    }

    [Fact]
    public void CreateProduct_WillCreateProduct()
    {
        //Arrange
        var productService = new ProductService();
        var newProduct = _fixture.Create<Product>();

        //Act
        productService.Create(newProduct);

        //Assert
        productService.GetAll().Should().HaveCount(1);
        productService.GetAll().Single().Should().Be(newProduct);
    }
}
