using Common.Attributes;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoWire(Lifetime.Transient, RegisterAs.Interface)]
internal class Validator2 : IValidator
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = "Validator2";

    public void Validate(ValidationContext validationContext)
    {
        var message = $"{Name} executed as Transient)";

        validationContext.AddCompletedValidation(Id, message);
    }
}
