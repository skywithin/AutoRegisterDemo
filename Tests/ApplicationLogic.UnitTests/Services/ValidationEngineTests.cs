using ApplicationLogic.Services.Validation;
using ApplicationLogic.Services.Validation.Validators;
using FluentAssertions;

namespace ApplicationLogic.UnitTests.Services;

internal class ValidationEngineTests : UnitTestContext<ValidationEngine>
{
    [Test]
    public void Execute_Should_Execute_Every_Supplied_Validator()
    {
        // Arrange
        var context = new ValidationContext(input: "test");

        InjectMock<IEnumerable<IValidator>>(
            new List<IValidator>
            {
                new LengthCheckValidator(),
                new UpperCaseValidator(),
            });

        // Act
        Sut.Execute(context);

        // Assert
        context.CompletedValidations.Should().HaveCount(2);

    }
}
