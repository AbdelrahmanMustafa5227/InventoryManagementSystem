using InventoryManagementSystem.Api.Filters;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Features.Transactions.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryManagementSystem.Api.Controllers
{
    [Route("transactions")]
    [ApiController]
    public class TransactionsController : ApiController
    {
        private readonly ISender _sender;

        public TransactionsController(ISender sender)
        {
            _sender = sender;
        }


        [Authorize]
        [ServiceFilter<IdempotencyFilter>]
        [HttpPost("addToStock")]
        public async Task<IActionResult> ActionResult([FromBody] AddToStockCommand command)
        {
            var result = await _sender.Send(command);
            return result.IsSuccess ? NoContent() : ToProblemDetails(result.Error!);
        }

        [Authorize]
        [ServiceFilter<IdempotencyFilter>]
        [HttpPost("removeFromStock")]
        public async Task<IActionResult> ActionResult([FromBody] RemoveFromStockCommand command)
        {
            var result = await _sender.Send(command);
            return result.IsSuccess ? NoContent() : ToProblemDetails(result.Error!);
        }

        [Authorize]
        [ServiceFilter<IdempotencyFilter>]
        [HttpPost("transfer")]
        public async Task<IActionResult> ActionResult([FromBody] TransferCommand command)
        {
            var result = await _sender.Send(command);
            return result.IsSuccess ? NoContent() : ToProblemDetails(result.Error!);
        }
    }
}
