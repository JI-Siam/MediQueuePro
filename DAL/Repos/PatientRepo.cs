using DAL.EF;
using DAL.EF.Tables;

public class PatientRepo
{
    private readonly MediQueueProDbContext db;

    public PatientRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }

    public Patient? GetByEmail(string email)
    {
        return db.Patients.FirstOrDefault(patient => patient.Email == email);
    }

    public Patient Add(Patient patient)
    {
        db.Patients.Add(patient);
        db.SaveChanges();
        return patient;
    }
}