using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

public class AppointMentRepo
{
    private readonly MediQueueProDbContext db;

    public AppointMentRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }

    public List<Appointment> GetAll()
    {
        return db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .OrderByDescending(a => a.CreatedAt)
            .ThenByDescending(a => a.AppointmentDate)
            .ThenByDescending(a => a.AppointmentTime)
            .ToList();
    }

    public List<Appointment> GetByPatientId(int patientId)
    {
        return db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.CreatedAt)
            .ThenByDescending(a => a.AppointmentDate)
            .ThenByDescending(a => a.AppointmentTime)
            .ToList();
    }

    public List<Appointment> GetByDoctorId(int doctorId)
    {
        return db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctorId)
            .OrderByDescending(a => a.CreatedAt)
            .ThenByDescending(a => a.AppointmentDate)
            .ThenByDescending(a => a.AppointmentTime)
            .ToList();
    }

    public List<Appointment> GetQueueByDoctorId(int doctorId, DateOnly? appointmentDate = null)
    {
        var date = appointmentDate ?? DateOnly.FromDateTime(DateTime.Today);

        return db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate == date)
            // keep non-completed first, then completed at the bottom
            .OrderBy(a => a.Status == "Completed")
            .ThenByDescending(a => a.IsEmergency == true)
            .ThenBy(a => a.QueueToken)
            .ToList();
    }

    public List<Appointment> GetQueueByPatientId(int patientId, DateOnly? appointmentDate = null)
    {
        var date = appointmentDate ?? DateOnly.FromDateTime(DateTime.Today);

        return db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.PatientId == patientId && a.AppointmentDate == date)
            // non-completed first, then completed at the bottom
            .OrderBy(a => a.Status == "Completed")
            .ThenByDescending(a => a.IsEmergency == true)
            .ThenBy(a => a.Doctor!.FullName)
            .ThenBy(a => a.QueueToken)
            .ToList();
    }

    public Appointment? GetById(int appointmentId)
    {
        return db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .FirstOrDefault(a => a.AppointmentId == appointmentId);
    }

    public int GetNextQueueToken(int doctorId, DateOnly appointmentDate)
    {
        var maxToken = db.Appointments
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate == appointmentDate)
            .Select(a => (int?)a.QueueToken)
            .Max() ?? 0;

        return maxToken + 1;
    }

    public bool HasPatientAppointmentOnDate(int patientId, int doctorId, DateOnly appointmentDate)
    {
        return db.Appointments.Any(a =>
            a.PatientId == patientId &&
            a.DoctorId == doctorId &&
            a.AppointmentDate == appointmentDate);
    }

    public Appointment Add(Appointment appointment)
    {
        appointment.CreatedAt ??= DateTime.Now;
        appointment.Status = string.IsNullOrWhiteSpace(appointment.Status) ? "Pending" : appointment.Status;
        appointment.IsEmergency ??= false;
        appointment.IsVisited ??= false;

        db.Appointments.Add(appointment);
        db.SaveChanges();

        return appointment;
    }

    public Appointment? UpdateStatus(int appointmentId, string status)
    {
        var appt = db.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
        if (appt == null) return null;

        // set status and visited/visit time when completed
        appt.Status = status;
        if (string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase))
        {
            appt.IsVisited = true;
            appt.VisitTime = DateTime.UtcNow;

            // move to end of queue by assigning next token for that doctor/date
            var next = GetNextQueueToken(appt.DoctorId, appt.AppointmentDate);
            appt.QueueToken = next;
        }
        else if (string.Equals(status, "Waiting", StringComparison.OrdinalIgnoreCase))
        {
            // Keep token but mark as waiting
            appt.IsVisited = false;
        }

        db.SaveChanges();
        return appt;
    }

    public bool Delete(int appointmentId)
    {
        var appointment = db.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

        if (appointment == null)
        {
            return false;
        }

        db.Appointments.Remove(appointment);
        db.SaveChanges();
        return true;
    }
}