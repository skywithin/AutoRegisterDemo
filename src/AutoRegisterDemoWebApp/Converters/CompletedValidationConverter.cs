using ApplicationLogic.Services.Validation;
using AutoRegisterDemoWebApp.Models;
using Common.Extensions.Datetime;

namespace AutoRegisterDemoWebApp.Converters;

public static class CompletedValidationConverter
{
    public static CompletedValidationModel ToCompletedValidationModel(this CompletedValidation source) =>
        new CompletedValidationModel(
            Result: source.Result,
            Message: $"{source.Timestamp.ToHHMMSS()}: {source.Message} (ID: {source.Id})");

    public static IEnumerable<CompletedValidationModel> ToCompletedValidationModels(this IEnumerable<CompletedValidation> source) =>
        source.Select(x => ToCompletedValidationModel(x));


}
