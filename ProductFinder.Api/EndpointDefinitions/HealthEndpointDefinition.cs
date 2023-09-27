using ProductFinder.Api.Extensions;

namespace ProductFinder.Api.EndpointDefinitions;

public class HealthEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/", CheckHealth).AllowAnonymous();
    }

    internal IResult CheckHealth()
    {
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
    }
}
