# ProductFinder.Api

## Setup requirement when using Visual Studio 2022 (non preview edition)
This is because I use dotnet core 8 (preview)

Tools -> Options
Environment -> Preview Features -> Check "Use Previews of the .NET SDK"

## Requirements
- Health check endpoints (accessible annonymously) are tested in HealthEndpointsTests.cs
- Get Product endpoints (accessible securely) are tested in ProductEndpointsTests.cs

## Other general information:
The new Identity API's were [approved](https://github.com/dotnet/aspnetcore/issues/49424) on August the 21st

Security Model:
[Auth and Identity in Dot Net Core 8](https://devblogs.microsoft.com/dotnet/improvements-auth-identity-aspnetcore-8/)


### In order to examine claims principle map this GET endpoint
```cli
app.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}")
    .RequireAuthorization();
```