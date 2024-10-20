using Common.Bootstrap;
using Common.Extensions;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
internal class Validator1 : IValidator
{
    public string Name => "AAA";

    public void Validate(ValidationContext validationContext)
    {
        var timestamp = DateTime.Now.ToHHMMSS();

        validationContext.AddCompletedValidation(
            $"{timestamp}: Validation completed by {Name}");
    }
}
