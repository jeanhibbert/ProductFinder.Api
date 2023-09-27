using ProductFinder.Api.Extensions;
using ProductFinder.Api.Models;
using ProductFinder.Api.Services;
using System.Drawing;

namespace ProductFinder.Api.EndpointDefinitions;

public class ProductEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/api/products", GetAllProducts);
        //.RequireAuthorization();
        app.MapGet("/api/products/{id}", GetProductById);
        app.MapPost("/api/products", CreateProduct);
        app.MapPut("/api/products/{id}", UpdateProduct);
        app.MapDelete("/api/products/{id}", DeleteProductById);
    }

    internal List<Product> GetAllProducts(IProductService service, HttpContext context)
    {
        if (Enum.TryParse(context.Request.Query["color"], ignoreCase: true, out ProductColor color))
        {
            return service.GetByColor(color);
        }

        return service.GetAll();
    }

    internal IResult GetProductById(IProductService service, Guid id)
    {
        var product = service.GetById(id);
        return product is not null ? Results.Ok(product) : Results.NotFound();
    }

    internal IResult CreateProduct(IProductService service, Product product)
    {
        service.Create(product);
        return Results.Created($"/products/{product.Id}", product);
    }

    internal IResult UpdateProduct(IProductService service, Guid id, Product updatedProduct)
    {
        var product = service.GetById(id);
        if (product is null)
        {
            return Results.NotFound();
        }

        service.Update(updatedProduct);
        return Results.Ok(updatedProduct);
    }

    internal IResult DeleteProductById(IProductService service, Guid id)
    {
        service.Delete(id);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<IProductService, ProductService>();
    }
}