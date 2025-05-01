using Hangfire;
using Hangfire.SqlServer;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(op => op.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductWarehouseRepository, ProductWarehouseRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Bearer");

            services.AddAuthorization();

            services.AddOptionsWithValidateOnStart<JwtOptions>()
                .Bind(configuration.GetSection(JwtOptions.SectionName))
                .ValidateDataAnnotations();

            services.ConfigureOptions<JwtOptionsSetup>();

            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));

            // Background Jobs
            services.AddHangfire(cfg => cfg.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
            services.AddScoped<ArchiveOldTransactionsJob>();
            services.AddScoped<LowStockNotificationJob>();

            services.AddMemoryCache();
            services.AddScoped<IdempotencyService>();
            services.Configure<PaginationOptions>(configuration.GetSection(PaginationOptions.SectionName));
            services.AddSingleton<ICachingService, CachingService>();
            return services;
        }
    }
}
