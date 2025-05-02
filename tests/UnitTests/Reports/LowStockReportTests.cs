using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Features.Reports.Queries;
using InventoryManagementSystem.Application.Helpers.Pagination;
using InventoryManagementSystem.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.Reports
{
    public class LowStockReportTests
    {
        private readonly IWarehouseRepository _warehouseRepository = Substitute.For<IWarehouseRepository>();
        private readonly IAppLogger<GetLowStockReportQueryHandler> _logger = Substitute.For<IAppLogger<GetLowStockReportQueryHandler>>();
        private readonly IUserContext _userContext = Substitute.For<IUserContext>();
        private readonly GetLowStockReportQueryHandler _sut;

        public LowStockReportTests()
        {
            _sut = new GetLowStockReportQueryHandler(_warehouseRepository, _logger, _userContext);
        }

        [Fact]
        public async Task Handle_ShouldReturnLowStockReport()
        {
            // Arrange
            var query = new GetLowStockReportQuery(1);
            var warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    Id = 1,
                    Name = "Warehouse 1",
                    ProductWarehouses = new List<ProductWarehouse>
                    {
                        new ProductWarehouse { ProductId = 1, Quantity = 5 , Product = new Product{ Name = "P1" }},
                        new ProductWarehouse { ProductId = 2, Quantity = 2 , Product = new Product{ Name = "P2" }}
                    }
                },
                new Warehouse
                {
                    Id = 2,
                    Name = "Warehouse 2",
                    ProductWarehouses = new List<ProductWarehouse>
                    {
                        new ProductWarehouse { ProductId = 3, Quantity = 0 , Product = new Product{ Name = "P3" } }
                    }
                }
            };
            var queryResult = new QueryResult<Warehouse>(warehouses, warehouses.Count);
            _warehouseRepository.GetWarehousesWithLowStockProducts(1).Returns(queryResult);
            // Act
            var result = await _sut.Handle(query, CancellationToken.None);
            // Assert
            _logger.Received(1).LogInformation("{0} has generated Low stock report successfully.", _userContext.GetLoggedUserEmail);
            Assert.True(result.IsSuccess);
            Assert.Equal(warehouses.Count, result.Value.TotalCount);
        }
    }
}
