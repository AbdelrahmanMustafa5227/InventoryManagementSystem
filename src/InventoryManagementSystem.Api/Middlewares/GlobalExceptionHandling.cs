using InventoryManagementSystem.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryManagementSystem.Api.Middlewares
{
    public class GlobalExceptionHandling
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var problemDetails = ex switch
                {
                    ValidationFailureException ve => new ProblemDetails()
                    {
                        Title = "Validation Faliure",
                        Status = (int)HttpStatusCode.BadRequest,
                        Detail = ve.Message,
                        Extensions = new Dictionary<string, object?>() { { "Failures", ve.ValidationFailures } }
                    },
                    _ => new ProblemDetails()
                    {
                        Title = "Server Error",
                        Status = (int)HttpStatusCode.InternalServerError,
                        Detail = ex.Message,
                    }
                };

                context.Response.StatusCode = (int)problemDetails.Status!;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
