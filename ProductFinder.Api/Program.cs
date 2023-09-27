using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProductFinder.Api.Extensions;
using ProductFinder.Persistence;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("AppDb"));

builder.Services.AddIdentityCore<MyUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

var app = builder.Build();
app.UseEndpointDefinitions();

// Adds /register, /login and /refresh endpoints
app.MapIdentityApi<MyUser>();

app.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}")
    .RequireAuthorization();

//app.UseHttpsRedirection();

app.UseHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

app.UseHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self") || r.Tags.Contains("dependency")
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

public partial class Program { }

