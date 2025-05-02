using InventoryManagementSystem.Api.Filters;
using InventoryManagementSystem.Api.Helpers;
using InventoryManagementSystem.Application.Abstractions.Services;
using Microsoft.AspNetCore.RateLimiting;

namespace InventoryManagementSystem.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddProblemDetails();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IdempotencyFilter>();

            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", opt =>
                {
                    opt.PermitLimit = 3;
                    opt.Window = TimeSpan.FromHours(1);
                });

                options.RejectionStatusCode = 429;
            });

            return services;
        }
    }
}
