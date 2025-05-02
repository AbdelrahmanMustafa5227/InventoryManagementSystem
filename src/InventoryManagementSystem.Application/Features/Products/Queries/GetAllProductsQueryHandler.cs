using InventoryManagementSystem.Application.Abstractions.Caching;
using InventoryManagementSystem.Application.Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Products.Queries
{
    public record GetAllProductsQuery(int PageNumber) : IRequest<Result<Paginated<GetAllProductsResponse>>>, ICachable
    {
        public string CacheKey => $"All-Products-{PageNumber}";
        public int DurationInSeconds => 60;
    }

    public record GetAllProductsResponse(long Id, string Name, string Description, decimal Price, int LowStockThreshold);

    internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<Paginated<GetAllProductsResponse>>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Result<Paginated<GetAllProductsResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var queryResult = await _productRepository.GetAllAsync(request.PageNumber);

            return Paginated<GetAllProductsResponse>.Create(
                queryResult.Items.ToResponse(),
                request.PageNumber , queryResult.TotalRecords
                );
        }

        public class GetAllProductsValidator : AbstractValidator<GetAllProductsQuery>
        {
            public GetAllProductsValidator()
            {
                RuleFor(x => x.PageNumber)
                    .GreaterThan(0)
                    .WithMessage("Page number must be greater than 0.");
            }
        }
    }

}
