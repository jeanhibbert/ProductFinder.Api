using ProductFinder.Api.Models;

namespace ProductFinder.Api.Services;

public interface IProductService
{
    List<Product> GetByColor(ProductColor color);

    List<Product> GetAll();

    void Create(Product product);

    Product GetById(Guid id);
}