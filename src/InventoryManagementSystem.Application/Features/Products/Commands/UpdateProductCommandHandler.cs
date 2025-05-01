using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Products.Commands
{
    public record UpdateProductCommand(long Id, string Name, string Description, decimal Price , int LowStockThreshold) : IRequest<Result>;

    internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
                return Result.Failure(Error.NotFound);

            product.ApplyChanges(request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull();

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
