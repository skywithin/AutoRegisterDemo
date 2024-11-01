using ApplicationLogic.Services.Validation;
using AutoRegisterDemoWebApp.Models;
using Common.Extensions.Datetime;
using Microsoft.AspNetCore.Mvc;

namespace AutoRegisterDemoWebApp.Controllers;
public class HomeController : Controller
{
    private readonly IValidationEngine _validationEngine;

    public HomeController(IValidationEngine validationEngine)
    {
        _validationEngine = validationEngine;
    }

    public IActionResult Index()
    {
        var validationContext = new ValidationContext();

        _validationEngine.Execute(validationContext);

        var viewModel =
            validationContext.CompletedValidations
                .Select(x =>
                    new CompletedValidationModel(
                        Message: $"{x.Timestamp.ToHHMMSS()}: {x.Message} (ID: {x.Id})"))
                .ToList();

        return View(viewModel);
    }
}
