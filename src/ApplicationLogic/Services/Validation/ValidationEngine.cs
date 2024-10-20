﻿using ApplicationLogic.Services.Validation.Validators;
using Common.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationLogic.Services.Validation;

public interface IValidationEngine
{
    ValidationContext Execute(ValidationContext context);
}

[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
internal class ValidationEngine : IValidationEngine
{
    private readonly IEnumerable<IValidator> _validators;

    public ValidationEngine(IServiceProvider serviceProvider)
    {
        _validators = serviceProvider.GetServices<IValidator>();
    }

    public ValidationContext Execute(ValidationContext context)
    {
        foreach (var validator in _validators)
        {
            validator.Validate(context);
        }

        return context;
    }
}
