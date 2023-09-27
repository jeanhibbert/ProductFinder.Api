using ProductFinder.Api.Extensions;
using ProductFinder.Api.Models;
using ProductFinder.Api.Services;

namespace ProductFinder.Api.EndpointDefinitions;

public class ProductEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/api/products", GetFilteredProducts).RequireAuthorization();
        app.MapPost("/api/products", CreateProduct).RequireAuthorization();
    }

    public IList<Product> GetFilteredProducts(IProductService service, IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        if (context.Request.Query.Any())
        {
            if (Enum.TryParse(context.Request.Query["color"], ignoreCase: true, out ProductColor color))
            {
                return service.GetByColor(color);
            }
            return new List<Product>();
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
        return Results.Created($"/api/products/{product.Id}", product);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<IProductService, ProductService>();
    }
}