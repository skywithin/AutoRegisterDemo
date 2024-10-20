﻿using Common.Bootstrap;
using Common.Extensions;

namespace ApplicationLogic.Services.Validation.Validators;

[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
internal class Validator3 : IValidator
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = "CCC";

    public void Validate(ValidationContext validationContext)
    {
        var timestamp = DateTime.Now.ToHHMMSS();

        validationContext.AddCompletedValidation(
            $"{timestamp}: Validation completed by {Name}. ID: {Id}");
    }
}