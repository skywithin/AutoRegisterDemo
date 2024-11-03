using ApplicationLogic.Services.Validation.Validators;
using Common.Attributes;

namespace ApplicationLogic.Services.Validation;

public interface IValidationEngine
{
    ValidationContext Execute(ValidationContext context);
}

[AutoWire(Lifetime.Scoped, RegisterAs.Interface)]
internal class ValidationEngine : IValidationEngine
{
    private readonly IEnumerable<IValidator> _validators;

    public ValidationEngine(IEnumerable<IValidator> validators)
    {
        _validators = validators;
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
