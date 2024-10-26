using Common.Attributes;
using Common.Extensions.Datetime;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoWire(Lifetime.Scoped, RegisterAs.Interface)]
internal class Validator3 : IValidator
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = "Validator3";

    public void Validate(ValidationContext validationContext)
    {
        var timestamp = DateTime.Now.ToHHMMSS();

        validationContext.AddCompletedValidation(
            $"{timestamp}: {Name} (ID: {Id}) executed (Scoped)");
    }
}
