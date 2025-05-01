using InventoryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Products.Queries
{
    public record GetDetailedProductQuery(long Id) : IRequest<Result<GetDetailedProductResponse>>;
    public record GetDetailedProductResponse(long Id, string Name, string Description, decimal Price, int LowStockThreshold , List<TransactionDTO> Transactions , List<WarehouseDTO> Warehouses);
    public record TransactionDTO(long Id, TransactionType Type, decimal Quantity);
    public record WarehouseDTO(long Id, int Quantity);

    internal class GetDetailedProductQueryHandler : IRequestHandler<GetDetailedProductQuery, Result<GetDetailedProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        public GetDetailedProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Result<GetDetailedProductResponse>> Handle(GetDetailedProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetDetailedByIdAsync(request.Id);

            if (product == null)
                return Result.Failure<GetDetailedProductResponse>(Error.NotFound);

            var response = product.ToDetailedResponse();
            return response;
        }

        public class GetDetailedProductQueryValidator : AbstractValidator<GetDetailedProductQuery>
        {
            public GetDetailedProductQueryValidator()
            {
                RuleFor(x => x.Id)
                    .NotNull();
            }
        }
    }
}
