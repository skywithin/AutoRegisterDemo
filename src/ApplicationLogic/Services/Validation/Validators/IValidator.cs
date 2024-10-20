namespace ApplicationLogic.Services.Validation.Validators;

public interface IValidator
{
    string Name { get; }

    void Validate(ValidationContext validationContext);
}
