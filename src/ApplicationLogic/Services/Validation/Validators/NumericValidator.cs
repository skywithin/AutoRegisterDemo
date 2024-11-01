using Common.Attributes;
using System.Text.RegularExpressions;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoWire(Lifetime.Scoped, RegisterAs.Interface)]
internal class NumericValidator : IValidator
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = "Numeric check";
    public string Pattern { get; } = "[0-9]";

    public void Validate(ValidationContext validationContext)
    {
        if (Regex.IsMatch(validationContext.Input, Pattern))
        {
            validationContext.AddCompletedValidation(
                Id,
                result: true,
                message: $"{Name}: Pass");
        }
        else
        {
            validationContext.AddCompletedValidation(
                Id,
                result: false,
                message: $"{Name}: Fail");
        }
    }
}
