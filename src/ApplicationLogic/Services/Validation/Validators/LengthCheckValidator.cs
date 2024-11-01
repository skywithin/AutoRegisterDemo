using Common.Attributes;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoWire(Lifetime.Singleton, RegisterAs.Interface)]
internal class LengthCheckValidator : IValidator
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = "Length check";
    public int MinLength { get; } = 6;

    public void Validate(ValidationContext validationContext)
    {
        if (validationContext.Input.Length >= MinLength)
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
