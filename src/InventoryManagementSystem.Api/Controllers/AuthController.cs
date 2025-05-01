using InventoryManagementSystem.Api.Filters;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.EmailTemplates;
using InventoryManagementSystem.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ApiController
    {
        private readonly ISender _sender;
        private readonly IEmailService emailService;

        public AuthController(ISender sender, IEmailService emailService)
        {
            _sender = sender;
            this.emailService = emailService;
        }

        [AllowAnonymous]
        [ServiceFilter<IdempotencyFilter>]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _sender.Send(command);
            return result.IsSuccess ? Ok() : ToProblemDetails(result.Error!);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error!);
        }

        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var result = await _sender.Send(new RevokeCommand());
            return result.IsSuccess ? NoContent() : ToProblemDetails(result.Error!);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromQuery] Guid guid)
        {
            var result = await _sender.Send(new RefreshCommand(guid));
            return result.IsSuccess ? Ok(result.Value) : ToProblemDetails(result.Error!);
        }

        [HttpGet("a")]
        public async Task<IActionResult> Get()
        {
            await emailService.SendAsync("Test" , "abdelrahman.mustafa5227@gmail.com" , RegisterEmailTemplate.Get("Abdo"));
            return Ok();
        }
    }
}
