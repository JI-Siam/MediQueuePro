using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

public class DoctorRepo
{
    MediQueueProDbContext db;

    public DoctorRepo(MediQueueProDbContext db)
    {
        this.db = db;
    }

    public Doctor Add(Doctor doctor)
    {
        db.Doctors.Add(doctor);
        db.SaveChanges();
        return doctor;
    }

    public Doctor? GetByEmail(string email)
    {
        return db.Doctors.FirstOrDefault(doctor => doctor.Email == email);
    }

    public List<Doctor> GetAll()
    {
        return db.Doctors
            .Include(doctor => doctor.Specialization)
            .OrderBy(doctor => doctor.FullName)
            .ToList();
    }

    public List<Doctor> GetFiltered(int? specializationId, bool? isAvailable)
    {
        var query = db.Doctors
            .Include(doctor => doctor.Specialization)
            .AsQueryable();

        if (specializationId.HasValue)
        {
            query = query.Where(doctor => doctor.SpecializationId == specializationId.Value);
        }

        if (isAvailable.HasValue)
        {
            query = query.Where(doctor => doctor.IsAvailable == isAvailable.Value);
        }

        return query
            .OrderBy(doctor => doctor.FullName)
            .ToList();
    }

    public Doctor? GetById(int doctorId)
    {
        return db.Doctors
            .Include(doctor => doctor.Specialization)
            .FirstOrDefault(doctor => doctor.DoctorId == doctorId);
    }


    public Doctor Update(Doctor doctor)
    {
        db.Doctors.Update(doctor);
        db.SaveChanges();
        return doctor;
    }

    public bool Delete(int doctorId)
    {
        var doc = db.Doctors.FirstOrDefault(d => d.DoctorId == doctorId);
        if (doc == null) return false;
        db.Doctors.Remove(doc);
        db.SaveChanges();
        return true;
    }
}