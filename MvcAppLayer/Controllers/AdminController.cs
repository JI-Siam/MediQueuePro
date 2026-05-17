using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    AdminService srvc;

    public AdminController(AdminService srvc)
    {
        this.srvc = srvc;
    }
    [AdminAccess]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}