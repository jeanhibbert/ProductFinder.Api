using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductFinder.Persistence;

namespace ProductFinder.Api.Extensions;

public static class DependencyExtensions
{
    public static void AddIdentityDbEndpoints(this IServiceCollection services)
    {
        services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        services.AddAuthorizationBuilder();

        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("AppDb"));

        services.AddIdentityCore<MyUser>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddApiEndpoints();
    }

    public static void AddHealthCheckEndpoints(this WebApplication app)
    {
        app.UseHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });
        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self") || r.Tags.Contains("dependency")
        });
    }
}
