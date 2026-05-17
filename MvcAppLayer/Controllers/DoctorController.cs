using Microsoft.AspNetCore.Mvc;

public class DoctorController : Controller
{
    DoctorService srvc;

    public DoctorController(DoctorService srvc)
    {
        this.srvc = srvc;
    }
    public IActionResult Index()
    {
        return View();
    }
}