using MediatR;
using ProductFinder.Api.Services;

namespace ProductFinder.Api.Handlers;

public record GetAllProductsRequest : IRequest<IResult>;

public class GetAllProductsRequestHandler
    : IRequestHandler<GetAllProductsRequest, IResult>
{
    private readonly IProductService _productService;

    public GetAllProductsRequestHandler(IProductService ProductService)
    {
        _productService = ProductService;
    }

    public Task<IResult> Handle(
        GetAllProductsRequest request, CancellationToken cancellationToken)
    {
        var products = _productService.GetAll();
        var response = Results.Ok(products);
        return Task.FromResult(response);
    }
}
