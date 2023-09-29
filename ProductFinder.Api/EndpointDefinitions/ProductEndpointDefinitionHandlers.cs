using MediatR;
using ProductFinder.Api.Extensions;
using ProductFinder.Api.Handlers;
using ProductFinder.Api.Models;
using ProductFinder.Api.Services;

namespace ProductFinder.Api.EndpointDefinitions;

public class ProductEndpointDefinitionHandlers
//  : IEndpointDefinition (Work in progress...)
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("api/products", async (IMediator mediator, GetAllProductsRequest request) => await mediator.Send(request));
        app.MapGet("api/products/{colorInput}", async (IMediator mediator, GetProductsByColorRequest request) => await mediator.Send(request))
            .AddEndpointFilter(async (invocationContext, next) =>
            {
                var colorInput = invocationContext.GetArgument<string>(0);
                if (!Enum.TryParse(colorInput, ignoreCase: true, out ProductColor color))
                {
                    return new List<Product>();
                }
                return await next(invocationContext);
            }).RequireAuthorization();
        app.MapPost("/customers", async (IMediator mediator, CreateProductRequest request) => await mediator.Send(request))
            .RequireAuthorization();
    }

    public IList<Product> GetProductsByColor(string colorInput, IProductService service)
    {
        if (Enum.TryParse(colorInput, ignoreCase: true, out ProductColor color))
        {
            return service.GetByColor(color);
        }
        return new List<Product>();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<IProductService, ProductService>();
    }
}