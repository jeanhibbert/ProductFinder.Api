using ProductFinder.Api.Models;
using System.Drawing;

namespace ProductFinder.Api.Services;

public interface IProductService
{
    List<Product> GetByColor(ProductColor color);

    List<Product> GetAll();

    void Create(Product product);

    Product GetById(Guid id);

    void Update(Product product);

    void Delete(Guid id);
}