using InventoryManagementSystem.Api.Filters;
using InventoryManagementSystem.Application.Features.Products.Commands;
using InventoryManagementSystem.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductsController : ApiController
    {
        private readonly ISender Sender;

        public ProductsController(ISender sender)
        {
            Sender = sender;
        }

        [HttpPost("add")]
        [ServiceFilter<IdempotencyFilter>]
        public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
        {
            var result = await Sender.Send(command);
            return result.IsSuccess ? CreatedAtAction(nameof(GetProductDetails), new { Id = result.Value.Id }, result.Value)
                : ToProblemDetails(result.Error!);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProduct([FromQuery] long id)
        {
            var command = new DeleteProductCommand(id);
            var result = await Sender.Send(command);
            return result.IsSuccess ? NoContent() : ToProblemDetails(result.Error!);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            var result = await Sender.Send(command);
            return result.IsSuccess ? NoContent() : ToProblemDetails(result.Error!);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetProductDetails([FromQuery] long id)
        {
            var query = new GetDetailedProductQuery(id);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error!);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts([FromQuery] int page)
        {
            var query = new GetAllProductsQuery(page);
            var result = await Sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error!);
        }
    }
}
