using InventoryManagementSystem.Application.Abstractions.Authentication;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using InventoryManagementSystem.Infrastructure.Authentication;
using InventoryManagementSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.IntegrationTests.Setup
{
    public class BaseTest : IClassFixture<AppFactory>
    {
        protected readonly HttpClient _httpClient;
        protected readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IServiceScope _serviceScope;

        public BaseTest(AppFactory factory)
        {
            _httpClient = factory.CreateClient();
            _serviceScope = factory.Services.CreateScope();
            _jwtTokenProvider = _serviceScope.ServiceProvider.GetRequiredService<IJwtTokenProvider>();
        }
    }
}
