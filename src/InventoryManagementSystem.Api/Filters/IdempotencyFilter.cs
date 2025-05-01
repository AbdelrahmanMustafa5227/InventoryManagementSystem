using InventoryManagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace InventoryManagementSystem.Api.Filters
{
    public class IdempotencyFilter : IAsyncActionFilter
    {
        private readonly IdempotencyService idempotencyService;

        public IdempotencyFilter(IdempotencyService service)
        {
            idempotencyService = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("X-Idempotency", out var idempotencyKey))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.HttpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Missing Headers",
                    Detail = "X-Idempotency Header is Missing",
                });
                return;
            }

            if (!Guid.TryParse(idempotencyKey, out var idempotencyId))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.HttpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Invalid Headers",
                    Detail = "X-Idempotency Header is Invalid",
                });
                return;
            }

            if (idempotencyService.IdempotentRequestExists(idempotencyId))
            {
                context.HttpContext.Response.StatusCode = idempotencyService.GetIdempotentRequest(idempotencyId);
                return;
            }

            await next();

            context.HttpContext.Response.OnCompleted(async () =>
            {
                if (context.HttpContext.Response.StatusCode < 400)
                    idempotencyService.CreateIdempotentRequest(idempotencyId, context.HttpContext.Response.StatusCode);
                await Task.CompletedTask;
            });
        }
    }
}
