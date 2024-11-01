using Common.Attributes;
using System.Text.RegularExpressions;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoWire(Lifetime.Transient, RegisterAs.Interface)]
internal class UpperCaseValidator : IValidator
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = "Upper case check";
    public string Pattern { get; } = "[A-Z]";

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
