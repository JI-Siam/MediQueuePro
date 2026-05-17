using DAL.EF;
using DAL.EF.Tables;

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
            .OrderByDescending(a => a.CreatedAt)
            .ThenByDescending(a => a.AppointmentDate)
            .ThenByDescending(a => a.AppointmentTime)
            .ToList();
    }

    public List<Appointment> GetQueueByDoctorId(int doctorId)
    {
        return db.Appointments
            .Where(a => a.DoctorId == doctorId)
            .OrderByDescending(a => a.IsEmergency == true)
            .ThenBy(a => a.QueueToken)
            .ToList();
    }

    public Appointment? GetById(int appointmentId)
    {
        return db.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
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