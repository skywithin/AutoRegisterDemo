namespace ApplicationLogic.Services.Validation;

public class ValidationContext
{
    private readonly List<CompletedValidation> _completedValidations = new();

    public IEnumerable<CompletedValidation> CompletedValidations => _completedValidations;

    public void AddCompletedValidation(Guid id, string message) =>
        _completedValidations.Add(
            new CompletedValidation(
                Id: id,
                Message: message,
                Timestamp: DateTime.Now));
}

public record CompletedValidation(
    Guid Id,
    string Message,
    DateTime Timestamp
);
