using ProductFinder.Api.Models;
using System.Drawing;

namespace ProductFinder.Api.Services;

public class ProductService : IProductService
{
    private readonly Dictionary<Guid, Product> _products = new();

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

    public List<Product> GetByColor(ProductColor color)
    {
        return _products
            .Values
            .Where(x => x.ProductColor.Equals(color))
            .ToList();
    }

    public List<Product> GetAll()
    {
        return _products.Values.ToList();
    }

    public void Update(Product product)
    {
        var existingProduct = GetById(product.Id);
        if (existingProduct is null)
        {
            return;
        }

        _products[product.Id] = product;
    }

    public void Delete(Guid id)
    {
        _products.Remove(id);
    }
}