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

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }




    public IActionResult List()
    {
        return View();
    }

    // update later 

    public IActionResult Queue(int id)
    {
        return View();
    }

}
