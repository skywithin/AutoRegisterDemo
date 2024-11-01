namespace ApplicationLogic.Services.Validation;

public class ValidationContext
{
    private readonly List<CompletedValidation> _completedValidations = new();

    public string Input {  get; private set; }

    public IEnumerable<CompletedValidation> CompletedValidations => _completedValidations;

    public ValidationContext(string input)
    {
        Input = input;
    }

    public void AddCompletedValidation(Guid id, bool result, string message) =>
        _completedValidations.Add(
            new CompletedValidation(
                Id: id,
                Result: result,
                Message: message,
                Timestamp: DateTime.Now));
}

public record CompletedValidation(
    Guid Id,
    bool Result,
    string Message,
    DateTime Timestamp
);
