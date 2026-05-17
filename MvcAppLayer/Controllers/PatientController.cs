using DAL.EF.Tables;
using Microsoft.AspNetCore.Mvc;

public class PatientController : Controller
{
    PatientService srvc;

    public PatientController(PatientService srvc)
    {
        this.srvc = srvc;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
}