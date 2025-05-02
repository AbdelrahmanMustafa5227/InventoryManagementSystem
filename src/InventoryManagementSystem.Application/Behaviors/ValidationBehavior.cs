using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IAppLogger<TRequest> _logger;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IAppLogger<TRequest> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationErrors = _validators
                .Select(x => x.Validate(context))
                .Where(x => x.Errors.Any())
                .SelectMany(x => x.Errors)
                .Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))
                .ToList();

            if (validationErrors.Any())
            {
                _logger.LogError("{0} Validation {1} Has Occurred on Request {2}\n",
                    validationErrors.Count,
                    validationErrors.Count == 1 ? "Error" : "Errors",
                    request.GetType().Name);
                throw new ValidationFailureException(validationFailures: validationErrors);
            }

            return await next();
        }
    }
}
