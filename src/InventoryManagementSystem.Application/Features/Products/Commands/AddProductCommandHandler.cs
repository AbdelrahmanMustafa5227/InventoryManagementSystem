using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Products.Commands
{
    public record AddProductCommand(string Name, string Description, decimal Price, int LowStockThreshold) : IRequest<Result<AddProductResponse>>;
    public record AddProductResponse(long Id ,string Name, string Description, decimal Price, int LowStockThreshold);


    public class AddProductCommandHandler : IRequestHandler<AddProductCommand,Result<AddProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAppLogger<AddProductCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AddProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IAppLogger<AddProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<AddProductResponse>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            Product product = request.ToModel();
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Product with Id {ProductId} Has Successfully been created", product.Id);
            return product.ToResponse();
        }
    }

    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(200);

            RuleFor(x => x.Price)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.LowStockThreshold)
                .NotNull()
                .GreaterThanOrEqualTo(0);
        }
    }
}
