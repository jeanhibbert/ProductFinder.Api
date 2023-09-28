using ProductFinder.Api.Extensions;
using ProductFinder.Api.Models;
using ProductFinder.Api.Services;

namespace ProductFinder.Api.EndpointDefinitions;

public class ProductEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/api/products", GetAll).RequireAuthorization();
        app.MapGet("api/products/{colorInput}", GetProductsByColor)
            .AddEndpointFilter(async (invocationContext, next) =>
            {
                var colorInput = invocationContext.GetArgument<string>(0);
                if (!Enum.TryParse(colorInput, ignoreCase: true, out ProductColor color))
                {
                    return new List<Product>();
                }
                return await next(invocationContext);
            }).RequireAuthorization(); 
        app.MapPost("/api/products", CreateProduct).RequireAuthorization();
    }

    public IList<Product> GetAll(IProductService service)
    {
        return service.GetAll();
    }

    public IList<Product> GetProductsByColor(string colorInput, IProductService service)
    {
        if (Enum.TryParse(colorInput, ignoreCase: true, out ProductColor color))
        {
            return service.GetByColor(color);
        }
        return new List<Product>();
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