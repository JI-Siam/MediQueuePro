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

    public List<Patient> GetAll()
    {
        return db.Patients
            .OrderBy(p => p.FullName)
            .ToList();
    }

    public Patient? GetById(int id)
    {
        return db.Patients.FirstOrDefault(p => p.PatientId == id);
    }

    public Patient Update(Patient patient)
    {
        db.Patients.Update(patient);
        db.SaveChanges();
        return patient;
    }

    public bool Delete(int id)
    {
        var p = db.Patients.FirstOrDefault(x => x.PatientId == id);
        if (p == null) return false;
        db.Patients.Remove(p);
        db.SaveChanges();
        return true;
    }

    public List<Doctor> GetAllDoctor()
    {
        return db.Doctors
            .OrderBy(doctor => doctor.FullName)
            .ToList();
    }
}