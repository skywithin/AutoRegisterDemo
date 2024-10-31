using ApplicationLogic.Services.Validation;
using FluentAssertions;

namespace ApplicationLogic.UnitTests.Services;

internal class ValidationContextTests
{
    [Test]
    public void AddCompletedValidation_Should_Add_New_Item_To_CompletedValidations()
    {
        // Arrange
        var sut = new ValidationContext();

        // Act
        sut.AddCompletedValidation(message: "Test1");
        sut.AddCompletedValidation(message: "Test2");
        sut.AddCompletedValidation(message: "Test3");

        // Assert
        sut.CompletedValidations.Should().HaveCount(3);
    }
}
