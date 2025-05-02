using InventoryManagementSystem.Application.Abstractions.Authentication;
using InventoryManagementSystem.Application.Abstractions.Caching;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Helpers.Pagination;
using InventoryManagementSystem.Infrastructure.Authentication;
using InventoryManagementSystem.Infrastructure.BackgroundJobs;
using InventoryManagementSystem.Infrastructure.Persistence;
using InventoryManagementSystem.Infrastructure.Persistence.Repositories;
using InventoryManagementSystem.Infrastructure.Services;
using InventoryManagementSystem.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Persistence
            services.AddDbContext<AppDbContext>(op => op.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductWarehouseRepository, ProductWarehouseRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

            // Authentication
            services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();


            // Configuration
            services.AddOptionsWithValidateOnStart<JwtOptions>()
                .Bind(configuration.GetSection(JwtOptions.SectionName))
                .ValidateDataAnnotations();
            services.ConfigureOptions<JwtOptionsSetup>();
            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
            services.Configure<PaginationOptions>(configuration.GetSection(PaginationOptions.SectionName));

            // Background Jobs
            services.RegisterHangfireJobs(configuration);

            // Other Services
            services.AddMemoryCache();
            services.AddScoped<IdempotencyService>();
            services.AddSingleton<ICachingService, CachingService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
