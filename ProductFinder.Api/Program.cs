using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProductFinder.Api.Extensions;
using ProductFinder.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));
builder.Services.AddIdentityDbEndpoints();

builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

var app = builder.Build();

app.UseEndpointDefinitions();
app.MapIdentityApi<MyUser>();

app.UseHttpsRedirection();
app.AddHealthCheckEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

public partial class Program { }

