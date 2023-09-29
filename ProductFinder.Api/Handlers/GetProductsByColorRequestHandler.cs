using MediatR;
using ProductFinder.Api.Models;
using ProductFinder.Api.Services;
using System.Text.Json.Serialization;

namespace ProductFinder.Api.Handlers;

public record GetProductsByColorRequest : IRequest<IResult>
{
    [JsonPropertyName("productColor")]
    public ProductColor ProductColor { get; init; } = default!;
}

public class GetProductsByColorRequestHandler
    : IRequestHandler<GetProductsByColorRequest, IResult>
{
    private readonly IProductService _productService;

    public GetProductsByColorRequestHandler(IProductService ProductService)
    {
        _productService = ProductService;
    }

    public Task<IResult> Handle(GetProductsByColorRequest request, CancellationToken cancellationToken)
    {
        var products = _productService.GetByColor(request.ProductColor);
        var response = Results.Ok(products);
        return Task.FromResult(response);
    }
}
