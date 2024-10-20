using Common.Bootstrap;
using Common.Extensions;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
internal class Validator2 : IValidator
{
    public string Name => "BBB";

    public void Validate(ValidationContext validationContext)
    {
        var timestamp = DateTime.Now.ToHHMMSS();

        validationContext.AddCompletedValidation(
            $"{timestamp}: Validation completed by {Name}");
    }
}
