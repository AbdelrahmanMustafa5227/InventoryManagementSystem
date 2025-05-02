using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Reports.Queries
{
    public record GetLowStockReportQuery(int page) : IRequest<Result<Paginated<WarehouseDto>>>;
    public record WarehouseDto(long Id, string Name, List<ProductDto> productsLowInStock);
    public record ProductDto(long Id, string Name, int Quantity);

    internal class GetLowStockReportQueryHandler : IRequestHandler<GetLowStockReportQuery, Result<Paginated<WarehouseDto>>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IAppLogger<GetLowStockReportQueryHandler> _logger;
        private readonly IUserContext _userContext;

        public GetLowStockReportQueryHandler(IWarehouseRepository warehouseRepository, IAppLogger<GetLowStockReportQueryHandler> logger, IUserContext userContext)
        {
            _warehouseRepository = warehouseRepository;
            _logger = logger;
            _userContext = userContext;
        }

        public async Task<Result<Paginated<WarehouseDto>>> Handle(GetLowStockReportQuery request, CancellationToken cancellationToken)
        {
            var queryResult = await _warehouseRepository.GetWarehousesWithLowStockProducts(request.page);

            var lowStockWarehouses = queryResult.Items
                .Select(w => new WarehouseDto(w.Id, w.Name, w.ProductWarehouses
                    .Select(p => new ProductDto(p.ProductId, p.Product.Name, p.Quantity))
                    .ToList()
                    ))
                .ToList();

            _logger.LogInformation("{0} has generated Low stock report successfully.", _userContext.GetLoggedUserEmail);
            return Paginated<WarehouseDto>.Create(lowStockWarehouses, request.page, queryResult.TotalRecords);
        }

        public class GetLowStockReportQueryValidator : AbstractValidator<GetLowStockReportQuery>
        {
            public GetLowStockReportQueryValidator()
            {
                RuleFor(x => x.page)
                    .GreaterThan(0)
                    .WithMessage("Page number must be greater than 0.");
            }
        }
    }
}
