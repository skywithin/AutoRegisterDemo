using ApplicationLogic.Services.Validation;
using AutoRegisterDemoWebApp.Converters;
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
        var validationContext = new ValidationContext(input: "");

        _validationEngine.Execute(validationContext);

        return View(validationContext.CompletedValidations.ToCompletedValidationModels());
    }

    [HttpGet("{input}")]
    public IActionResult Index(string input)
    {
        var validationContext = new ValidationContext(input);

        _validationEngine.Execute(validationContext);

        return View(validationContext.CompletedValidations.ToCompletedValidationModels());
    }
}
