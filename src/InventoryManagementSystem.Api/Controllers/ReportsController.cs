using InventoryManagementSystem.Application.Features.Reports.Queries;
using InventoryManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers
{
    [Route("reports")]
    [ApiController]
    public class ReportsController : ApiController
    {
        private readonly ISender _sender;

        public ReportsController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("lowStock")]
        public async Task<IActionResult> GetLowStockReport([FromQuery]int page)
        {
            var result = await _sender.Send(new GetLowStockReportQuery(page));
            return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error!);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("transactionHistory")]
        public async Task<IActionResult> GetTransactionHistoryReport([FromQuery] DateTime From, DateTime To, TransactionType TransactionType, int page)
        {
            var result = await _sender.Send(new GetTransactionHistoryQuery(From, To, TransactionType, page));
            return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error!);
        }
    }
}
