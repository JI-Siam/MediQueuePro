using Microsoft.AspNetCore.Mvc;

public class DoctorController : Controller
{
    private readonly DoctorService srvc;
    private readonly AppointMentService appointmentService;

    public DoctorController(DoctorService srvc, AppointMentService appointmentService)
    {
        this.srvc = srvc;
        this.appointmentService = appointmentService;
    }

    [DoctorAccess]
    public IActionResult Index()
    {
        // get the names and type stored in session
        ViewBag.Uname = HttpContext.Session.GetString("Uname");
        ViewBag.UType = HttpContext.Session.GetInt32("UType");
        return View();
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

        var doctor = srvc.Login(dto);

        if (doctor == null)
        {
            ViewBag.Error = "Invalid email or password.";
            return View(dto);
        }

        HttpContext.Session.SetString("Uname", doctor.FullName);
        HttpContext.Session.SetInt32("UType", 3);
        HttpContext.Session.SetInt32("UID", doctor.DoctorId);

        return RedirectToAction("Index");
    }


    [DoctorAccess]
    public IActionResult List()
    {
        var doctors = srvc.GetAll();
        return View(doctors);
    }

    [DoctorAccess]
    [HttpGet]
    public IActionResult Edit()
    {
        var doctorId = HttpContext.Session.GetInt32("UID");
        if (doctorId == null)
        {
            return RedirectToAction("Login");
        }

        var model = srvc.GetEditModel(doctorId.Value);
        if (model == null)
        {
            return NotFound();
        }

        ViewBag.Specializations = srvc.GetSpecializations();
        ViewBag.DoctorId = doctorId.Value;
        return View(model);
    }

    [DoctorAccess]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(DoctorRegDTO formObj)
    {
        var doctorId = HttpContext.Session.GetInt32("UID");
        if (doctorId == null)
        {
            return RedirectToAction("Login");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Specializations = srvc.GetSpecializations();
            ViewBag.DoctorId = doctorId.Value;
            return View(formObj);
        }

        var updated = srvc.Update(doctorId.Value, formObj);
        if (updated == null)
        {
            ViewBag.Specializations = srvc.GetSpecializations();
            ViewBag.DoctorId = doctorId.Value;
            ViewBag.Error = "Unable to update doctor profile.";
            return View(formObj);
        }

        HttpContext.Session.SetString("Uname", updated.FullName);
        TempData["Message"] = "Profile updated successfully.";
        return RedirectToAction("Index");
    }



    [DoctorAccess]
    public IActionResult Appointments()
    {
        var doctorId = HttpContext.Session.GetInt32("UID");
        if (doctorId == null)
        {
            return RedirectToAction("Login");
        }

        return View(appointmentService.GetByDoctorId(doctorId.Value));
    }

    [DoctorAccess]
    public IActionResult AppointmentDetails(int id)
    {
        var doctorId = HttpContext.Session.GetInt32("UID");
        if (doctorId == null)
        {
            return RedirectToAction("Login");
        }

        var appointment = appointmentService.GetById(id);
        if (appointment == null || appointment.DoctorId != doctorId.Value)
        {
            return NotFound();
        }

        return View(appointment);
    }

    [DoctorAccess]
    public IActionResult Queue(int id)
    {
        ViewBag.DoctorId = id;
        return View(appointmentService.GetQueueByDoctorId(id));
    }



    [HttpPost]
    [DoctorAccess]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateStatus(int appointmentId, string status)
    {
        var doctorId = HttpContext.Session.GetInt32("UID");
        if (doctorId == null) return RedirectToAction(nameof(Login));

        var appointment = appointmentService.GetById(appointmentId);
        if (appointment == null || appointment.DoctorId != doctorId.Value)
        {
            return Forbid();
        }

        var updated = appointmentService.UpdateAppointmentStatus(appointmentId, status);
        if (updated == null)
        {
            TempData["Error"] = "Unable to update status.";
            return RedirectToAction("Queue", new { id = doctorId.Value });
        }

        return RedirectToAction("Queue", new { id = doctorId.Value });
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
