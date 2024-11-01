using Common.Attributes;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoWire(Lifetime.Singleton, RegisterAs.Interface)]
internal class Validator1 : IValidator
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = "Validator1";

    public void Validate(ValidationContext validationContext)
    {
        var message = $"{Name} executed as Singleton";

        validationContext.AddCompletedValidation(Id, message);
    }
}
