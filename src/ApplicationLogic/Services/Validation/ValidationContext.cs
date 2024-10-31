namespace ApplicationLogic.Services.Validation;
public class ValidationContext
{
    private readonly List<string> _completedValidations = new();

    public IEnumerable<string> CompletedValidations => _completedValidations;

    public void AddCompletedValidation(string message) =>
        _completedValidations.Add(message);
}
