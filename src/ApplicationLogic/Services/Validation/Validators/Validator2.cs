using Common.Bootstrap;
using Common.Extensions;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoWire(Lifetime.Transient, RegisterAs.Interface)]
internal class Validator2 : IValidator
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = "Validator2";

    public void Validate(ValidationContext validationContext)
    {
        var timestamp = DateTime.Now.ToHHMMSS();

        validationContext.AddCompletedValidation(
            $"{timestamp}: {Name} (ID: {Id}) executed (Transient)");
    }
}
