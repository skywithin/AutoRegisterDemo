namespace ApplicationLogic.Services.Validation.Validators;

public interface IValidator
{
    Guid Id { get; }
    string Name { get; }

    void Validate(ValidationContext validationContext);
}
