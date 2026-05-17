using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    AdminService srvc;

    public AdminController(AdminService srvc)
    {
        this.srvc = srvc;
    }
    public IActionResult Index()
    {
        return View();
    }
}