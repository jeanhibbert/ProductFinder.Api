using MediatR;
using ProductFinder.Api.Models;
using ProductFinder.Api.Services;
using System.Text.Json.Serialization;

namespace ProductFinder.Api.Handlers;

public record CreateProductRequest : IRequest<IResult>
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; } = Guid.NewGuid();

    [JsonPropertyName("productName")]
    public string ProductName { get; init; } = default!;

    [JsonPropertyName("productColor")]
    public ProductColor ProductColor { get; init; } = default!;
}

public class CreateProductRequestHandler
    : IRequestHandler<CreateProductRequest, IResult>
{
    private readonly IProductService _productService;

    public CreateProductRequestHandler(IProductService productService)
    {
        _productService = productService;
    }

    public Task<IResult> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = request.Id,
            ProductName = request.ProductName,
            ProductColor = request.ProductColor
        };
        _productService.Create(product);
        var response = Results.Created($"api/products/{product.Id}", product);
        return Task.FromResult(response);
    }
}