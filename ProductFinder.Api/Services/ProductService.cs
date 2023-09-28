using ProductFinder.Api.Models;

namespace ProductFinder.Api.Services;

public class ProductService : IProductService
{
    private readonly Dictionary<Guid, Product> _products = new();

    public ProductService() { }

    public ProductService(Dictionary<Guid, Product> products)
    {
        ArgumentNullException.ThrowIfNull(products, nameof(products));

        _products = products;
    }

    public List<Product> GetByColor(ProductColor color)
    {
        return _products.Values
            .Where(x => x.ProductColor.Equals(color))
            .ToList();
    }

    public List<Product> GetAll()
    {
        return _products.Values.ToList();
    }

    public void Create(Product product)
    {
        if (product is null)
        {
            return;
        }

        _products[product.Id] = product;
    }

    public Product GetById(Guid id)
    {
        return _products.GetValueOrDefault(id);
    }
}