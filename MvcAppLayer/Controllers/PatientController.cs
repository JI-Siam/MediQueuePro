using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class PatientController : Controller
{
    private readonly PatientService srvc;

    public PatientController(PatientService srvc)
    {
        this.srvc = srvc;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new PatientRegDTO());
    }

    [HttpPost]
    public IActionResult Register(PatientRegDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var createdPatient = srvc.Register(dto);
        if (createdPatient == null)
        {
            ViewBag.Error = "Email already exists.";
            return View(dto);
        }
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginDTO());
    }

    [HttpPost]
    public IActionResult Login(LoginDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var patient = srvc.Login(dto);

        if (patient == null)
        {
            ViewBag.Error = "Invalid email or password.";
            return View(dto);
        }

        HttpContext.Session.SetString("Uname", patient.FullName);
        HttpContext.Session.SetInt32("UType", 2);
        HttpContext.Session.SetInt32("UID", patient.PatientId);

        return RedirectToAction(nameof(Dashboard));
    }

    [Logged]
    public IActionResult Dashboard()
    {
        ViewBag.Uname = HttpContext.Session.GetString("Uname");
        ViewBag.UType = HttpContext.Session.GetInt32("UType");
        ViewBag.Doctors = new List<string>();

        return View();
    }
}