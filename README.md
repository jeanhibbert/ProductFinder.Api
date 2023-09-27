# ProductFinder.Api

## Test Requirements
Health check endpoints (accessible annonymously) are tested in : HealthEndpointsTests

Security Model:
[Auth and Identity in Dot Net Core 8](https://devblogs.microsoft.com/dotnet/improvements-auth-identity-aspnetcore-8/)

Get all products (secured)


## Other general information:
The new Identity API's were approved on August the 21st
https://github.com/dotnet/aspnetcore/issues/49424

### In order to test claims map this get endpoint
```cli
app.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}")
    .RequireAuthorization();
```