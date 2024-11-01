using Common.Attributes;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoWire(Lifetime.Scoped, RegisterAs.Interface)]
internal class Validator3 : IValidator
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = "Validator3";

    public void Validate(ValidationContext validationContext)
    {
        var message = $"{Name} executed as Scoped";

        validationContext.AddCompletedValidation(Id, message);
    }
}
