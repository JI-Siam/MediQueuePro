using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly AdminService adminService;
    private readonly PatientService patientService;
    private readonly DoctorService doctorService;
    private readonly AppointMentService appointmentService;

    public AdminController(AdminService adminService, PatientService patientService, DoctorService doctorService, AppointMentService appointmentService)
    {
        this.adminService = adminService;
        this.patientService = patientService;
        this.doctorService = doctorService;
        this.appointmentService = appointmentService;
    }

    [AdminAccess]
    public IActionResult Index()
    {
        return View(adminService.GetAnalyticsDashboard());
    }

    [AdminAccess]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [AdminAccess]
    [HttpGet]
    public IActionResult DoctorCreate()
    {
        ViewBag.Specializations = doctorService.GetSpecializations();
        return View(new DoctorRegDTO());
    }

    [AdminAccess]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DoctorCreate(DoctorRegDTO formObj)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Specializations = doctorService.GetSpecializations();
            return View(formObj);
        }

        doctorService.Add(formObj);
        TempData["Message"] = "Doctor created successfully.";
        return RedirectToAction(nameof(Doctors));
    }

    [NotLogged]
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginDTO());
    }

    [NotLogged]
    [HttpPost]
    public IActionResult Login(LoginDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var admin = adminService.Login(dto);
        if (admin == null)
        {
            ViewBag.Error = "Invalid email or password.";
            return View(dto);
        }

        HttpContext.Session.SetString("Uname", admin.FullName);
        HttpContext.Session.SetInt32("UType", 1);
        HttpContext.Session.SetInt32("UID", admin.AdminId);

        return RedirectToAction(nameof(Index));
    }

    [AdminAccess]
    public IActionResult Patients()
    {
        var patients = patientService.GetAll();
        return View(patients);
    }

    [AdminAccess]
    public IActionResult PatientEdit(int id)
    {
        var model = patientService.GetEditModel(id);
        if (model == null) return NotFound();
        return View("../Patient/Edit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AdminAccess]
    public IActionResult PatientEdit(int id, PatientRegDTO dto)
    {
        var updated = patientService.UpdateFromReg(id, dto);
        if (updated == null) return NotFound();
        return RedirectToAction("Patients");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AdminAccess]
    public IActionResult PatientDelete(int id)
    {
        patientService.Delete(id);
        return RedirectToAction("Patients");
    }

    // Doctors CRUD
    [AdminAccess]
    public IActionResult Doctors()
    {
        var doctors = doctorService.GetAll();
        return View(doctors);
    }

    [AdminAccess]
    public IActionResult DoctorEdit(int id)
    {
        var model = doctorService.GetEditModel(id);
        if (model == null) return NotFound();
        return View("../Doctor/Edit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AdminAccess]
    public IActionResult DoctorEdit(int id, DoctorRegDTO dto)
    {
        var updated = doctorService.Update(id, dto);
        if (updated == null) return NotFound();
        return RedirectToAction("Doctors");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AdminAccess]
    public IActionResult DoctorDelete(int id)
    {
        doctorService.Delete(id);
        return RedirectToAction("Doctors");
    }


    [AdminAccess]
    public IActionResult Analytics()
    {
        var analyticsData = adminService.GetAnalyticsDashboard();
        return View(analyticsData);
    }

    [AdminAccess]
    public IActionResult DoctorPatients(int id)
    {
        ViewBag.Title = "Doctor-wise unique patients";
        ViewBag.ScopeName = doctorService.GetById(id)?.FullName ?? "Doctor";
        return View("PatientDetails", adminService.GetUniquePatients(doctorId: id));
    }

    [AdminAccess]
    public IActionResult SpecializationPatients(int id)
    {
        ViewBag.Title = "Specialization-wise unique patients";
        ViewBag.ScopeName = doctorService.GetSpecializations().FirstOrDefault(specialization => specialization.SpecializationId == id)?.Name ?? "Specialization";
        return View("PatientDetails", adminService.GetUniquePatients(specializationId: id));
    }
}