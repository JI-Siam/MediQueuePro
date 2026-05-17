using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class PatientController : Controller
{
    private readonly PatientService srvc;
    private readonly DoctorService doctorService;
    private readonly AppointMentService appointmentService;

    public PatientController(PatientService srvc, DoctorService doctorService, AppointMentService appointmentService)
    {
        this.srvc = srvc;
        this.doctorService = doctorService;
        this.appointmentService = appointmentService;
    }

    [PatientAccess]
    public IActionResult Index()
    {
        return View();
    }

    [NotLogged]
    [HttpGet]
    public IActionResult Register()
    {
        return View(new PatientRegDTO());
    }

    [NotLogged]
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

    [PatientAccess]
    public IActionResult Dashboard(int? specializationId, string? status)
    {
        ViewBag.Uname = HttpContext.Session.GetString("Uname");
        ViewBag.UType = HttpContext.Session.GetInt32("UType");

        bool? isAvailable = status?.ToLowerInvariant() switch
        {
            "available" => true,
            "unavailable" => false,
            _ => null
        };

        ViewBag.Specializations = doctorService.GetSpecializations();
        ViewBag.SelectedSpecializationId = specializationId;
        ViewBag.SelectedStatus = status;

        var doctors = doctorService.GetFiltered(specializationId, isAvailable);
        return View(doctors);
    }

    [PatientAccess]
    public IActionResult DoctorDetails(int id)
    {
        var doctor = doctorService.GetById(id);

        if (doctor == null)
        {
            return NotFound();
        }

        return View(doctor);
    }

    [PatientAccess]
    [HttpGet]
    public IActionResult BookAppointment(int doctorId)
    {
        var doctor = doctorService.GetById(doctorId);

        if (doctor == null)
        {
            return NotFound();
        }

        ViewBag.Doctor = doctor;
        return View(new AppointmentCreateDTO
        {
            DoctorId = doctorId,
            AppointmentDate = DateOnly.FromDateTime(DateTime.Today)
        });
    }

    [PatientAccess]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult BookAppointment(AppointmentCreateDTO dto)
    {
        dto.AppointmentDate = DateOnly.FromDateTime(DateTime.Today);

        if (!ModelState.IsValid)
        {
            ViewBag.Doctor = doctorService.GetById(dto.DoctorId);
            return View(dto);
        }

        var patientId = HttpContext.Session.GetInt32("UID");
        if (patientId == null)
        {
            return RedirectToAction(nameof(Login));
        }

        var appointment = appointmentService.Book(patientId.Value, dto);
        if (appointment == null)
        {
            ViewBag.Doctor = doctorService.GetById(dto.DoctorId);
            ViewBag.Error = "You already have an appointment with this doctor today.";
            return View(dto);
        }

        TempData["AppointmentToken"] = appointment.QueueToken;
        return RedirectToAction(nameof(Queue), new { id = appointment.AppointmentId });
    }

    [PatientAccess]
    public IActionResult Appointments()
    {
        var patientId = HttpContext.Session.GetInt32("UID");
        if (patientId == null)
        {
            return RedirectToAction(nameof(Login));
        }

        return View(appointmentService.GetByPatientId(patientId.Value));
    }

    [PatientAccess]
    public IActionResult Queues(int? doctorId)
    {
        var patientId = HttpContext.Session.GetInt32("UID");
        if (patientId == null)
        {
            return RedirectToAction(nameof(Login));
        }

        var doctors = doctorService.GetAll();
        ViewBag.Doctors = doctors;
        ViewBag.CurrentPatientId = patientId.Value;

        if (doctors.Count == 0)
        {
            ViewBag.SelectedDoctorId = null;
            return View(new List<AppointmentDTO>());
        }

        var selectedDoctorId = doctorId ?? doctors[0].DoctorId;
        ViewBag.SelectedDoctorId = selectedDoctorId;

        return View(appointmentService.GetQueueByDoctorId(selectedDoctorId));
    }

    [PatientAccess]
    public IActionResult AppointmentDetails(int id)
    {
        var patientId = HttpContext.Session.GetInt32("UID");
        if (patientId == null)
        {
            return RedirectToAction(nameof(Login));
        }

        var appointment = appointmentService.GetById(id);
        if (appointment == null || appointment.PatientId != patientId.Value)
        {
            return NotFound();
        }

        return View(appointment);
    }

    [PatientAccess]
    public IActionResult Queue(int id)
    {
        var appointment = appointmentService.GetById(id);
        if (appointment == null)
        {
            return NotFound();
        }

        var patientId = HttpContext.Session.GetInt32("UID");
        if (patientId == null || appointment.PatientId != patientId.Value)
        {
            return NotFound();
        }

        ViewBag.CurrentAppointment = appointment;
        return View(appointmentService.GetQueueByDoctorId(appointment.DoctorId));
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}